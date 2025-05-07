using System.Net;
using AutoMapper;
using BusinessObjects.Enums;
using BusinessObjects.Models;
using DataAccess.WalletDTO;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTService _jwtService;
        private readonly IMapper _mapper;

        public WalletService(IUnitOfWork unitOfWork, IJWTService jwtService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        // Lấy ví theo UserId từ JWT
        public async Task<WalletResModel> GetWalletById(int walletId)
        {
            var wallet = await _unitOfWork.Wallet.GetByIdAsync(walletId);
            if (wallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Wallet not found.");

            return _mapper.Map<WalletResModel>(wallet);
        }

        // Tạo ví mới
        public async Task<WalletResModel> CreateWallet(string token, WalletReqModel model)
        {
            var userId = Convert.ToInt32(_jwtService.decodeToken(token, "userId"));
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            // Kiểm tra nếu người dùng là shop
            if (user.RoleId == 6)
            {
                // Lấy ShopId từ bảng Shops (giả sử bảng Shops có trường ShopId liên kết với UserId)
                var shop = await _unitOfWork.Shop.FirstOrDefaultAsync(s => s.UserId == userId);
                if (shop == null)
                    throw new ApiException(HttpStatusCode.NotFound, "Shop not found.");

                // Kiểm tra nếu shop đã có ví
                var existingShopWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);
                if (existingShopWallet != null)
                    throw new ApiException(HttpStatusCode.BadRequest, "Shop already has a wallet.");

                // Tạo ví cho shop
                var shopWallet = _mapper.Map<Wallet>(model);
                shopWallet.UserId = user.UserId;
                shopWallet.ShopId = shop.ShopId;  // Gán ShopId vào ví của shop
                shopWallet.Balance = 0;
                shopWallet.Status = WalletEnum.Active.ToString();
                shopWallet.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.Wallet.AddAsync(shopWallet);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<WalletResModel>(shopWallet);
            }
            else
            {
                // Tạo ví cho user bình thường
                var existingWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.UserId == userId);
                if (existingWallet != null)
                    throw new ApiException(HttpStatusCode.BadRequest, "User already has a wallet.");

                var wallet = _mapper.Map<Wallet>(model);
                wallet.UserId = user.UserId;
                wallet.Balance = 0;
                wallet.Status = WalletEnum.Active.ToString();
                wallet.CreatedDate = DateTime.UtcNow;

                await _unitOfWork.Wallet.AddAsync(wallet);
                await _unitOfWork.SaveAsync();

                return _mapper.Map<WalletResModel>(wallet);
            }
        }



        public async Task<List<WalletResModel>> GetAllWallets()
        {
            var wallets = await _unitOfWork.Wallet.GetAllAsync();
            return _mapper.Map<List<WalletResModel>>(wallets);
        }

        public async Task DisburseToManagerAsync(decimal amount)
        {
            var managerWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.Type == "Manager");
            if (managerWallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Manager wallet not found.");

            // Cộng tiền vào ví
            managerWallet.Balance += amount;
            _unitOfWork.Wallet.Update(managerWallet);
         
            await _unitOfWork.SaveAsync();
        }


        public async Task WithdrawFromManagerAsync(decimal amount)
        {
            var managerWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.Type == "Manager");
            if (managerWallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Manager wallet not found.");

            if (managerWallet.Balance < amount)
                throw new ApiException(HttpStatusCode.BadRequest, "Manager wallet does not have enough balance.");

            // Trừ tiền từ ví
            managerWallet.Balance -= amount;
            _unitOfWork.Wallet.Update(managerWallet);


            await _unitOfWork.SaveAsync();
        }

        public async Task WithdrawFromShopAsync(string token, int shopId, decimal amount)
        {
            var userId = Convert.ToInt32(_jwtService.decodeToken(token, "userId"));
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            // 1. Lấy ví của Shop
            var shopWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.ShopId == shopId && w.Type == "Shop");
            if (shopWallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Shop wallet not found.");

            if (shopWallet.UserId != userId)
            {
                throw new ApiException(HttpStatusCode.Unauthorized, "User does not have permission to withdraw from this shop wallet.");
            }

            // 2. Kiểm tra số dư
            if (shopWallet.Balance < amount)
                throw new ApiException(HttpStatusCode.BadRequest, "Shop wallet does not have enough balance.");

            // 3. Trừ tiền từ ví Shop
            shopWallet.Balance -= amount;
            _unitOfWork.Wallet.Update(shopWallet);

            // 4. Ghi log rút tiền (ShopWithdraw)
            var shop = await _unitOfWork.Shop.FirstOrDefaultAsync(s => s.ShopId == shopId);
            if (shop == null)
                throw new ApiException(HttpStatusCode.NotFound, "Shop not found.");

            var shopWithdrawLog = new TransactionLog
            {
                UserId = shopWallet.UserId,
                TransactionType = "ShopWithdraw",
                Amount = -amount,
                CreatedDate = DateTime.UtcNow,
                Note = $"Shop #{shopId} ({shop.ShopName}) withdrew {amount}",
                ReferenceId = shopWallet.WalletId,
                SourceTable = "Wallet"
            };
            await _unitOfWork.TransactionLog.AddAsync(shopWithdrawLog);

            await _unitOfWork.SaveAsync();
        }

        public async Task TransferFromManagerToShopsAsync(List<ShopTransferReqModel> transfers, decimal feeRate)
        {
           
            var managerWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.Type == "Manager");
            if (managerWallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Manager wallet not found.");

            feeRate /= 100;

            decimal totalTransfer = 0;
            var preparedTransfers = new List<(Wallet ShopWallet, decimal Amount, decimal Fee, decimal ActualAmount, int ShopId)>();

            foreach (var transfer in transfers)
            {
                var shopWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w =>
                    w.ShopId == transfer.ShopId && w.Type == "Shop");

                if (shopWallet == null)
                    throw new ApiException(HttpStatusCode.NotFound, $"Shop wallet not found for ShopId {transfer.ShopId}.");

                // Tính phí và số tiền thực nhận
                var fee = Math.Round(transfer.Amount * feeRate, 2);
                var actual = transfer.Amount - fee;

                preparedTransfers.Add((shopWallet, transfer.Amount, fee, actual, transfer.ShopId));
                totalTransfer += actual;
            }

            // Kiểm tra ví Manager có đủ số dư để thực hiện chuyển tiền không
            if (managerWallet.Balance < totalTransfer)
                throw new ApiException(HttpStatusCode.BadRequest, $"Manager wallet has insufficient balance (needs {totalTransfer}).");

            // Thực hiện giao dịch
            foreach (var (shopWallet, originalAmount, fee, actualAmount, shopId) in preparedTransfers)
            {
                // Trừ tiền từ ví Manager
                managerWallet.Balance -= actualAmount;
                _unitOfWork.Wallet.Update(managerWallet);

                // Cộng tiền vào ví Shop
                shopWallet.Balance += actualAmount;
                _unitOfWork.Wallet.Update(shopWallet);

                // Lấy thông tin shop
                var shop = await _unitOfWork.Shop.FirstOrDefaultAsync(s => s.ShopId == shopId);
                if (shop == null)
                    throw new ApiException(HttpStatusCode.NotFound, "Shop not found.");

                // Ghi log giao dịch từ Manager (TransferOut)
                var managerTransactionLog = new TransactionLog
                {
                    UserId = managerWallet.UserId,
                    TransactionType = "TransferOut",  
                    Amount = -actualAmount,
                    CreatedDate = DateTime.UtcNow,
                    Note = $"Transferred {actualAmount} to Shop: {shop.ShopName} (Fee: {fee})",
                    ReferenceId = shopWallet.WalletId,
                    SourceTable = "Wallet"
                };

                var shopTransactionLog = new TransactionLog
                {
                    UserId = shopWallet.UserId,
                    TransactionType = "TransferIn",  
                    Amount = actualAmount,
                    CreatedDate = DateTime.UtcNow,
                    Note = $"Received {actualAmount} from Manager. Shop: {shop.ShopName} (Fee deducted: {fee})",
                    ReferenceId = managerWallet.WalletId,
                    SourceTable = "Wallet"
                };

                // Thêm log giao dịch của Manager và Shop
                await _unitOfWork.TransactionLog.AddAsync(managerTransactionLog);
                await _unitOfWork.TransactionLog.AddAsync(shopTransactionLog);                         

            }

            // Lưu tất cả các thay đổi vào DB
            await _unitOfWork.SaveAsync();
        }



    }
}

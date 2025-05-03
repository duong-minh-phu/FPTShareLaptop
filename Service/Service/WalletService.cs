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


        public async Task TransferFromManagerToShopsAsync(string token, List<ShopTransferReqModel> transfers, decimal feeRate)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var managerWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.Type == "Manager");
            if (managerWallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Manager wallet not found.");

            feeRate /= 100;

            decimal totalTransfer = 0;
            var preparedTransfers = new List<(Wallet ShopWallet, decimal Amount, decimal Fee, decimal ActualAmount, int ShopUserId)>();

            foreach (var transfer in transfers)
            {
                var shopWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w =>
                    w.UserId == transfer.ShopUserId && w.Type == "Shop");

                if (shopWallet == null)
                    throw new ApiException(HttpStatusCode.NotFound, $"Shop wallet not found for user ID {transfer.ShopUserId}.");

                var fee = Math.Round(transfer.Amount * feeRate, 2);
                var actual = transfer.Amount - fee;

                preparedTransfers.Add((shopWallet, transfer.Amount, fee, actual, transfer.ShopUserId));
                totalTransfer += actual;
            }

            if (managerWallet.Balance < totalTransfer)
                throw new ApiException(HttpStatusCode.BadRequest, $"Manager wallet has insufficient balance (needs {totalTransfer}).");

            // Thực hiện giao dịch
            foreach (var (shopWallet, originalAmount, fee, actualAmount, shopUserId) in preparedTransfers)
            {
                managerWallet.Balance -= actualAmount;
                shopWallet.Balance += actualAmount;

                _unitOfWork.Wallet.Update(managerWallet);
                _unitOfWork.Wallet.Update(shopWallet);

                var shop = await _unitOfWork.Shop.FirstOrDefaultAsync(s => s.UserId == shopWallet.UserId);
                if (shop == null)
                    throw new ApiException(HttpStatusCode.NotFound, "Shop not found.");

                var managerTransactionLog = new TransactionLog
                {
                    UserId = managerWallet.UserId,
                    TransactionType = "TransferOut",
                    Amount = -actualAmount,
                    CreatedDate = DateTime.UtcNow,
                    Note = $"Transferred {originalAmount} to ShopUserId {shopUserId}. Shop: {shop.ShopName} (Fee: {fee})",
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

                await _unitOfWork.TransactionLog.AddAsync(managerTransactionLog);
                await _unitOfWork.TransactionLog.AddAsync(shopTransactionLog);
            }

            await _unitOfWork.SaveAsync();
        }


    }
}

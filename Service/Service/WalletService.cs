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

            // Tạo WalletTransaction
            var transaction = new WalletTransaction
            {
                WalletId = managerWallet.WalletId,
                TransactionType = "Disbursement",
                Amount = amount,
                CreatedDate = DateTime.UtcNow,
                Note = "Disbursement after successful payment"
            };
            await _unitOfWork.WalletTransaction.AddAsync(transaction);

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

            // Ghi log giao dịch trừ tiền
            var transaction = new WalletTransaction
            {
                WalletId = managerWallet.WalletId,
                TransactionType = "Refund",
                Amount = -amount,
                CreatedDate = DateTime.UtcNow,
                Note = "Refund from compensation"
            };
            await _unitOfWork.WalletTransaction.AddAsync(transaction);

            await _unitOfWork.SaveAsync();
        }


        public async Task TransferFromManagerToShopAsync(string token, decimal amount, decimal feeRate)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            // 1. Tìm ví Manager theo WalletType
            var managerWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.Type == "Manager");
            if (managerWallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Manager wallet not found.");

            // 2. Tìm ví Shop theo WalletType
            var shopWallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.Type == "Shop");
            if (shopWallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Shop wallet not found.");

            // 3. Kiểm tra số dư
            if (managerWallet.Balance < amount)
                throw new ApiException(HttpStatusCode.BadRequest, "Manager wallet has insufficient balance.");
            
            //Chuyển feeRate từ phần trăm sang số thực
            feeRate /= 100;

            // 4. Tính phí và số tiền thực chuyển
            var fee = Math.Round(amount * feeRate, 2); 
            var transferAmount = amount - fee;

            // 5. Cập nhật số dư ví
            managerWallet.Balance -= transferAmount;  
            shopWallet.Balance += transferAmount;  

            _unitOfWork.Wallet.Update(managerWallet);
            _unitOfWork.Wallet.Update(shopWallet);

            // 6. Ghi lịch sử giao dịch
            var managerTransaction = new WalletTransaction
            {
                WalletId = managerWallet.WalletId,
                TransactionType = "TransferOut",
                Amount = -transferAmount,
                CreatedDate = DateTime.UtcNow,
                Note = $"Transferred {amount} to Shop (Fee: {fee})"
            };

            var shopTransaction = new WalletTransaction
            {
                WalletId = shopWallet.WalletId,
                TransactionType = "TransferIn",
                Amount = transferAmount,               
                CreatedDate = DateTime.UtcNow,
                Note = $"Received {transferAmount} from Manager (Fee deducted: {fee})"
            };
            Console.WriteLine($"Manager transaction: {System.Text.Json.JsonSerializer.Serialize(managerTransaction)}");
            Console.WriteLine($"Shop transaction: {System.Text.Json.JsonSerializer.Serialize(shopTransaction)}");

            await _unitOfWork.WalletTransaction.AddAsync(managerTransaction);
            await _unitOfWork.WalletTransaction.AddAsync(shopTransaction);

            await _unitOfWork.SaveAsync();
        }
    }
}

using System.Net;
using AutoMapper;
using BusinessObjects.Models;
using DataAccess.WalletTransaction;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class WalletTransactionService : IWalletTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJWTService _jwtService;
        private readonly IMapper _mapper;

        public WalletTransactionService(IUnitOfWork unitOfWork, IJWTService jwtService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _mapper = mapper;
        }

        // Lấy danh sách giao dịch theo UserId từ JWT
        public async Task<List<WalletTransactionResModel>> GetTransactionsByUser(string token)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");
            var wallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.UserId == user.UserId);
            if (wallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Wallet not found.");

            var transactions = await _unitOfWork.WalletTransaction.GetAllAsync(t => t.WalletId == wallet.WalletId);
            return _mapper.Map<List<WalletTransactionResModel>>(transactions);
        }

        // Lấy giao dịch theo ID
        public async Task<WalletTransactionResModel> GetTransactionById(int transactionId)
        {
            var transaction = await _unitOfWork.WalletTransaction.GetByIdAsync(transactionId);
            if (transaction == null)
                throw new ApiException(HttpStatusCode.NotFound, "Transaction not found.");

            return _mapper.Map<WalletTransactionResModel>(transaction);
        }

        // Tạo giao dịch mới
        public async Task<WalletTransactionResModel> CreateTransaction(string token, WalletTransactionReqModel model)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");
            var wallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.UserId == user.UserId);
            if (wallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Wallet not found.");

            var transaction = new WalletTransaction
            {
                WalletId = wallet.WalletId,
                TransactionType = model.TransactionType,
                Amount = model.Amount,
                CreatedDate = DateTime.UtcNow,
                RelatedPaymentId = model.RelatedPaymentId,
                Note = model.Note
            };

            await _unitOfWork.WalletTransaction.AddAsync(transaction);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<WalletTransactionResModel>(transaction);
        }

        // Xóa giao dịch
        public async Task DeleteTransaction(string token, int transactionId)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");
            var wallet = await _unitOfWork.Wallet.FirstOrDefaultAsync(w => w.UserId == user.UserId);
            if (wallet == null)
                throw new ApiException(HttpStatusCode.NotFound, "Wallet not found.");

            var transaction = await _unitOfWork.WalletTransaction.GetByIdAsync(transactionId);
            if (transaction == null || transaction.WalletId != wallet.WalletId)
                throw new ApiException(HttpStatusCode.NotFound, "Transaction not found or unauthorized.");

            _unitOfWork.WalletTransaction.Delete(transaction);
            await _unitOfWork.SaveAsync();
        }
    }
}

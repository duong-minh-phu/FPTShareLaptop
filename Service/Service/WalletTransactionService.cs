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

        // Xóa giao dịch
        public async Task DeleteTransaction(int transactionId)
        {          
            var transaction = await _unitOfWork.WalletTransaction.GetByIdAsync(transactionId);
            if (transaction == null)
                throw new ApiException(HttpStatusCode.NotFound, "Transaction not found or unauthorized.");

            _unitOfWork.WalletTransaction.Delete(transaction);
            await _unitOfWork.SaveAsync();
        }

        //Lấy tất cả giao dịch
        public async Task<List<WalletTransactionResModel>> GetAllTransactions()
        {
            var transactions = await _unitOfWork.WalletTransaction.GetAllAsync();
            var result = _mapper.Map<List<WalletTransactionResModel>>(transactions);
            return result;
        }
    }
}

using BusinessObjects.Models;
using DataAccess.RefundTransactionDTO;
using Service.IService;
using AutoMapper;
using System.Security.Claims;
using DataAccess.UserDTO;
using Service.Utils.CustomException;
using System.Net;

namespace Service.Service
{
    public class RefundTransactionService : IRefundTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jwtService;

        public RefundTransactionService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jwtService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<List<RefundTransactionResModel>> GetAllAsync()
        {
            var refunds = await _unitOfWork.RefundTransaction.GetAllAsync();
            return _mapper.Map<List<RefundTransactionResModel>>(refunds);
        }

        public async Task<RefundTransactionResModel> GetByIdAsync(int id)
        {
            var refund = await _unitOfWork.RefundTransaction.GetByIdAsync(id);
            return _mapper.Map<RefundTransactionResModel>(refund);
        }

        public async Task<List<RefundTransactionResModel>> GetByWalletIdAsync(int walletId)
        {
            var refunds = await _unitOfWork.RefundTransaction.GetAllAsync(r => r.WalletId == walletId);
            return _mapper.Map<List<RefundTransactionResModel>>(refunds);
        }

        public async Task AddAsync(RefundTransactionReqModel refundReq)
        {
            var refund = _mapper.Map<RefundTransaction>(refundReq);
            refund.CreatedDate = DateTime.UtcNow;
            refund.Status = "Pending";
            await _unitOfWork.RefundTransaction.AddAsync(refund);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(string token, RefundTransactionReqModel refundReq, int refundId)
        {
            var refund = await _unitOfWork.RefundTransaction.GetByIdAsync(refundId);
            if (refund == null) throw new KeyNotFoundException("RefundTransaction not found");

            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            _mapper.Map(refundReq, refund);
            _unitOfWork.RefundTransaction.Update(refund);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(string token, int refundId)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var refund = await _unitOfWork.RefundTransaction.GetByIdAsync(refundId);
            if (refund == null) throw new KeyNotFoundException("RefundTransaction not found");          

            _unitOfWork.RefundTransaction.Delete(refund);
            await _unitOfWork.SaveAsync();
        }

       
    }
}
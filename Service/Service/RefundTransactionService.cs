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


        public async Task AddAsync(string token, RefundTransactionReqModel refundReq)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");
           
            var deposit = await _unitOfWork.DepositTransaction.GetByIdAsync(refundReq.DepositId);
            if (deposit == null)
                throw new ApiException(HttpStatusCode.NotFound, "Deposit transaction not found.");
            
            var damage = await _unitOfWork.ReportDamage.GetByIdAsync(refundReq.ReportId);
            if (damage == null)
                throw new ApiException(HttpStatusCode.NotFound, "Damage report not found.");

            var refund = _mapper.Map<RefundTransaction>(refundReq);
            refund.RefundDate = DateTime.UtcNow;
            refund.Status = "Pending";
            refund.UserId = int.Parse(userId);

            // Logic xử lý hoàn tiền cọc
            decimal refundAmount;
            string refundNote;

            if (damage == null || damage.DamageFee == 0)
            {
                // Trường hợp không có thiệt hại, hoàn lại toàn bộ cọc
                refundAmount = deposit.Amount;
                refundNote = "Full refund as no damage was reported.";
            }
            else if (damage.DamageFee < deposit.Amount)
            {
                // Trường hợp thiệt hại nhỏ hơn cọc, hoàn lại phần dư sau khi trừ thiệt hại
                refundAmount = deposit.Amount - damage.DamageFee;
                refundNote = $"Refund after damage deduction: {damage.DamageFee} fee. Remaining refund: {refundAmount}.";
            }
            else
            {
                // Trường hợp thiệt hại lớn hơn hoặc bằng cọc, không hoàn lại
                refundAmount = 0;
                refundNote = $"No refund as damage fee ({damage.DamageFee}) exceeds or equals the deposit amount.";
            }

            // Gán số tiền hoàn lại và ghi chú vào đối tượng RefundTransaction
            refund.RefundAmount = refundAmount;
            refund.RefundNote = refundNote;

            // Thêm RefundTransaction vào cơ sở dữ liệu
            await _unitOfWork.RefundTransaction.AddAsync(refund);
            await _unitOfWork.SaveAsync();

            // Thêm transaction log
            var log = new TransactionLog
            {
                UserId = int.Parse(userId), // Ghi nhận UserId từ token
                TransactionType = "Refund", // Loại giao dịch là Refund
                Amount = refund.RefundAmount, // Số tiền refund
                CreatedDate = DateTime.UtcNow, // Thời gian tạo log
                Note = $"Refund transaction for contract {refund.ContractId}, amount: {refund.RefundAmount}", // Ghi chú
                ReferenceId = refund.RefundTransactionId, // Sử dụng RefundId làm ReferenceId
                SourceTable = "RefundTransaction" // Ghi lại bảng nguồn
            };

            await _unitOfWork.TransactionLog.AddAsync(log);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(RefundTransactionReqModel refundReq, int refundId)
        {
            var refund = await _unitOfWork.RefundTransaction.GetByIdAsync(refundId);
            if (refund == null) throw new KeyNotFoundException("RefundTransaction not found");
        
            _mapper.Map(refundReq, refund);
            _unitOfWork.RefundTransaction.Update(refund);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int refundId)
        {          

            var refund = await _unitOfWork.RefundTransaction.GetByIdAsync(refundId);
            if (refund == null) throw new KeyNotFoundException("RefundTransaction not found");          

            _unitOfWork.RefundTransaction.Delete(refund);
            await _unitOfWork.SaveAsync();
        }

       
    }
}
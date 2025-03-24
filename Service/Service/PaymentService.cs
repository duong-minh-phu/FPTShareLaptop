using System.Net;
using AutoMapper;
using BusinessObjects.Models;
using DataAccess.PaymentDTO;
using Microsoft.EntityFrameworkCore;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jwtService;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper, IJWTService jWTService)
        {            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtService = jWTService;
        }

        public async Task<List<PaymentResModel>> GetAllAsync()
        {
            var payments = await _unitOfWork.Payment.GetAllAsync();
            return _mapper.Map<List<PaymentResModel>>(payments);
        }

        public async Task<PaymentResModel?> GetByIdAsync(int id)
        {
            var payment = await _unitOfWork.Payment.GetByIdAsync(id);
            return _mapper.Map<PaymentResModel>(payment);
        }

        public async Task<List<PaymentResModel>> GetByOrderIdAsync(int orderId)
        {
            var payments = await _unitOfWork.Payment.GetByIdAsync(orderId);
            return _mapper.Map<List<PaymentResModel>>(payments);
        }

        public async Task AddAsync(PaymentReqModel request)
        {
            var payment = _mapper.Map<Payment>(request);
            payment.PaymentDate = DateTime.UtcNow;
            payment.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.Payment.AddAsync(payment);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(string token, int paymentId, PaymentReqModel request)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var payment = await _unitOfWork.Payment.GetByIdAsync(paymentId);
            if (payment == null) throw new Exception("Payment not found.");

            _mapper.Map(request, payment);
            payment.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Payment.Update(payment);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(string token, int paymentId)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var payment = await _unitOfWork.Payment.GetByIdAsync(paymentId);
            if (payment == null) throw new Exception("Payment not found.");

            _unitOfWork.Payment.Delete(payment);
            await _unitOfWork.SaveAsync();
        }
    }
}

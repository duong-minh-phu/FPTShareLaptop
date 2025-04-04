using System.Net;
using AutoMapper;
using BusinessObjects.Models;
using DataAccess.PaymentMethodDTO;
using Service.IService;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IJWTService _jwtService;

        public PaymentMethodService(IMapper mapper, IUnitOfWork unitOfWork, IJWTService jwtService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<List<PaymentMethodResModel>> GetAllAsync()
        {
            var paymentMethods = await _unitOfWork.PaymentMethod.GetAllAsync();
            return _mapper.Map<List<PaymentMethodResModel>>(paymentMethods);
        }

        public async Task<PaymentMethodResModel> GetByIdAsync(int id)
        {
            var paymentMethod = await _unitOfWork.PaymentMethod.GetByIdAsync(id);
            return _mapper.Map<PaymentMethodResModel>(paymentMethod);
        }

        public async Task<PaymentMethodResModel> AddAsync(PaymentMethodReqModel model)
        {
            var paymentMethod = _mapper.Map<PaymentMethod>(model);
            paymentMethod.CreatedDate = DateTime.UtcNow;
            paymentMethod.IsActive = true;
            await _unitOfWork.PaymentMethod.AddAsync(paymentMethod);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<PaymentMethodResModel>(paymentMethod);

        }

        public async Task UpdateAsync(string token, int id, PaymentMethodReqModel model)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var paymentMethod = await _unitOfWork.PaymentMethod.GetByIdAsync(id);
            if (paymentMethod != null)
            {
                _mapper.Map(model, paymentMethod);
                _unitOfWork.PaymentMethod.Update(paymentMethod);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task DeleteAsync(string token, int id)
        {
            var userId = _jwtService.decodeToken(token, "userId");
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null)
                throw new ApiException(HttpStatusCode.NotFound, "User not found.");

            var paymentMethod = await _unitOfWork.PaymentMethod.GetByIdAsync(id); 
            if (paymentMethod != null)
            {
                _unitOfWork.PaymentMethod.Delete(paymentMethod); 
                await _unitOfWork.SaveAsync();
            }
        }
    }
}

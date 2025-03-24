using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.PaymentDTO;

namespace Service.IService
{
    public interface IPaymentService
    {
        Task<List<PaymentResModel>> GetAllAsync();
        Task<PaymentResModel> GetByIdAsync(int id);
        Task<List<PaymentResModel>> GetByOrderIdAsync(int orderId);
        Task AddAsync(PaymentReqModel request);
        Task UpdateAsync(string token, int paymentId, PaymentReqModel request);
        Task DeleteAsync(string token, int paymentId);
    }
}

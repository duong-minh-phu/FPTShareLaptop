using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.PaymentDTO;
using DataAccess.PayOSDTO;
using Microsoft.AspNetCore.Http;

namespace Service.IService
{
    public interface IPaymentService
    {
        Task<List<PaymentViewResModel>> GetAllPayment();
        Task<PaymentViewResModel> GetPaymentByIdAsync(int paymentId);
        Task<int> CreatePaymentAsync(string token, int orderId, int paymentMethodId);
        Task<string> GetPaymentUrlAsync(HttpContext context, int paymentId, string redirectUrl);
        Task<bool> UpdatePaymentAsync(int paymentId);
    }
}

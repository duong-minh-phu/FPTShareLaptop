using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.PaymentDTO;
using DataAccess.PayOSDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;

namespace Service.IService
{
    public interface IPaymentService
    {
        Task<List<PaymentViewResModel>> GetAllPayment();
        Task<PaymentViewResModel> GetPaymentByIdAsync(int paymentId);
        Task<PaymentViewResModel> CreatePaymentAsync(string token, int orderId, int paymentMethodId);
        Task<string> GetPaymentUrlAsync(HttpContext context, int paymentId, string redirectUrl);
        Task HandlePaymentWebhookAsync(WebhookType webhookBody);
        Task UpdatePayment(string transactionCode, UpdatePaymentReqModel model);

    }
}

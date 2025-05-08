using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.PayOSDTO;
using Net.payOS.Types;


namespace Service.IService
{
    public interface IPayOSService
    {
        Task<CreatePaymentResult> CreatePaymentUrl(PayOSReqModel payOSReqModel);
        Task<WebhookData> VerifyPaymentWebhookData(WebhookType webhookBody);      
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.PayOSDTO;
using Microsoft.Extensions.Configuration;
using Net.payOS.Types;
using Net.payOS;
using Service.IService;
using System.Threading;
using Service.Utils.CustomException;

namespace Service.Service
{
    public class PayOSService : IPayOSService
    {
        private readonly IConfiguration _config;

        public PayOSService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<CreatePaymentResult> CreatePaymentUrl(PayOSReqModel payOSReqModel)
        {
            PayOS payOS = new PayOS(_config["PayOS:ClientID"], _config["PayOS:ApiKey"], _config["PayOS:ChecksumKey"]);

            ItemData item = new ItemData(payOSReqModel.ProductName, 1, (int)payOSReqModel.Amount);
            List<ItemData> items = new List<ItemData> { item };
            PaymentData paymentData = new PaymentData(payOSReqModel.OrderId, (int)payOSReqModel.Amount, "Thanh toán", items, payOSReqModel.CancelUrl, payOSReqModel.RedirectUrl);

            var paymentTask = payOS.createPaymentLink(paymentData);

            var timeoutTask = Task.Delay(TimeSpan.FromMinutes(15));

            var completedTask = await Task.WhenAny(paymentTask, timeoutTask);

            if (completedTask == timeoutTask)
            {               
                throw new ApiException(System.Net.HttpStatusCode.RequestTimeout, "The request to PayOS timed out.");
            }
            return await paymentTask;
        }
    }
}

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
            List<ItemData> items = new List<ItemData>();
            items.Add(item);
            PaymentData paymentData = new PaymentData(payOSReqModel.OrderId, (int)payOSReqModel.Amount, "Thanh toán ", items, payOSReqModel.CancelUrl, payOSReqModel.RedirectUrl);

            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);

            return createPayment;
        }
    }
}

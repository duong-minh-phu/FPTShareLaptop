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
using Net.payOS.Utils;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace Service.Service
{
    public class PayOSService : IPayOSService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<PayOSService> _logger;

        public PayOSService(IConfiguration config, ILogger<PayOSService> logger)
        {
            _config = config;
            _logger = logger;

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

        public async Task<WebhookData> VerifyPaymentWebhookData(WebhookType webhookBody)
        {
            _logger.LogInformation("VerifyPaymentWebhookData called.");

            if (webhookBody == null)
            {
                _logger.LogError("Webhook body is null.");
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Webhook body is null.");
            }

            var data = webhookBody.data;
            var signature = webhookBody.signature;

            if (data == null)
            {
                _logger.LogError("Webhook data is missing.");
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Webhook data is missing.");
            }

            if (string.IsNullOrEmpty(signature))
            {
                _logger.LogError("Webhook signature is missing.");
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Webhook signature is missing.");
            }

            _logger.LogInformation($"Webhook data received: {data.ToString()}");

            var checksumKey = _config["PayOS:ChecksumKey"];
            var generatedSignature = SignatureControl.CreateSignatureFromObj(JObject.FromObject(data), checksumKey);

            _logger.LogInformation($"Generated signature: {generatedSignature}");
            _logger.LogInformation($"Received signature: {signature}");

            if (generatedSignature != signature)
            {
                _logger.LogError("Invalid signature. Webhook data cannot be trusted.");
                throw new ApiException(System.Net.HttpStatusCode.BadRequest, "Invalid signature. Webhook data cannot be trusted.");
            }

            return data;
        }

    }
}

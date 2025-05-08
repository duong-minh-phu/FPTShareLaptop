using DataAccess.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using Net.payOS;
using System.Net;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayOSController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PayOSController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentLinkInformation(long orderCode)
        {
            PayOS payOS = new PayOS(_config["PayOS:ClientID"], _config["PayOS:ApiKey"], _config["PayOS:ChecksumKey"]);

            PaymentLinkInformation paymentLinkInformation = await payOS.getPaymentLinkInformation(orderCode);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get payment link information successfully",
                Data = paymentLinkInformation
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhookAsync([FromBody] string webhookUrl)
        {
            if (string.IsNullOrEmpty(webhookUrl))
            {
                return BadRequest(new { message = "Webhook URL is required." });
            }

            try
            {
                PayOS payOS = new PayOS(_config["PayOS:ClientID"], _config["PayOS:ApiKey"], _config["PayOS:ChecksumKey"]);

                var confirmationResult = await payOS.confirmWebhook(webhookUrl);

                if (confirmationResult != null)
                {
                    // Bạn có thể trả kèm dữ liệu confirmResult về cho dễ debug
                    return Ok(new { message = "Webhook URL has been successfully confirmed.", data = confirmationResult });
                }
                else
                {
                    return StatusCode(500, new { message = "Failed to confirm the webhook URL. PayOS returned an unexpected response." });
                }
            }
            catch (Net.payOS.Errors.PayOSError payOSError)
            {
                return StatusCode(500, new { message = $"PayOS error occurred: {payOSError.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }



    }
}

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
    }
}

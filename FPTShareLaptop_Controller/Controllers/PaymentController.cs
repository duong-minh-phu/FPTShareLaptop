using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataAccess.PaymentDTO;
using DataAccess.ResultModel;
using System.Net;
using System.Threading.Tasks;
using Net.payOS.Types;
using DataAccess.PayOSDTO;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Lấy danh sách tất cả Payment
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPayments()
        {
            var result = await _paymentService.GetAllPayment();
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Payments retrieved successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        /// Lấy thông tin payment theo ID
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentById(int paymentId)
        {

            var result = await _paymentService.GetPaymentByIdAsync(paymentId);
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Lấy thông tin thanh toán thành công.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        /// Lấy URL thanh toán cho payment
        [HttpGet("{paymentId}/payment-url")]
        public async Task<IActionResult> GetPaymentUrl(int paymentId, [FromQuery] string redirectUrl)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _paymentService.GetPaymentUrlAsync(HttpContext, paymentId, redirectUrl);
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Url retrieved successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        // Tạo thanh toán mới
        [HttpPost("create")]

        public async Task<IActionResult> CreatePaymentAsync(int orderID, int paymenMethodId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _paymentService.CreatePaymentAsync(token, orderID ,paymenMethodId);

            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Payment created successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        /// Xác nhận thanh toán
        [HttpPost("webhook")]
        public async Task<IActionResult> PaymentCallback([FromBody] PayOSWebhookModel callbackData)
        {
                await _paymentService.ProcessPaymentCallback(callbackData);
                var response = new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Payment processed successfully."
                };
                return StatusCode(response.Code, response);
        }
    }
}

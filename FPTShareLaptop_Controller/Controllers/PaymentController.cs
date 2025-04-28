using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataAccess.PaymentDTO;
using DataAccess.ResultModel;
using System.Net;
using System.Threading.Tasks;
using Net.payOS.Types;
using DataAccess.PayOSDTO;
using Newtonsoft.Json;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;


        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
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
                Message = "Payment detail  retrieved successfully.",
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
            var result = await _paymentService.CreatePaymentAsync(token, orderID, paymenMethodId);
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Payment created successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        /// Xác nhận thanh toán webhook
        [HttpPost("webhook")]
        public async Task<IActionResult> HandlePaymentWebhookAsync([FromBody] WebhookType webhookBody)
        {
            try
            {
                _logger.LogInformation($"Received webhook: {JsonConvert.SerializeObject(webhookBody)}");
                await _paymentService.HandlePaymentWebhookAsync(webhookBody);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi ở đây
                _logger.LogError(ex, "An error occurred while processing the payment webhook.");
            }

            // Trả kèm dữ liệu webhook nhận được
            return Ok(new
            {
                message = "Webhook received successfully.",
                receivedData = webhookBody
            });
        }

        [HttpPut("update/{transactionCode}")]
        public async Task<IActionResult> UpdatePaymentAsync(string transactionCode, [FromBody] UpdatePaymentReqModel model)
        {
            // Gọi service để cập nhật trạng thái thanh toán
            await _paymentService.UpdatePayment(transactionCode, model);

            // Trả về kết quả thành công
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Payment status updated successfully."
            };

            return StatusCode(response.Code, response);

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataAccess.PaymentDTO;
using DataAccess.ResultModel;
using System.Net;
using System.Threading.Tasks;

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
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var result = await _paymentService.GetAllAsync();
            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Payments retrieved successfully.",
                Data = result
            });
        }

        // Lấy Payment theo Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var result = await _paymentService.GetByIdAsync(id);
            if (result == null)
                return NotFound(new ResultModel { IsSuccess = false, Code = (int)HttpStatusCode.NotFound, Message = "Payment not found." });

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Payment retrieved successfully.",
                Data = result
            });
        }

        // Tạo mới Payment
        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _paymentService.AddAsync(request);
            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "Payment created successfully."
            });
        }

        // Cập nhật Payment
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePayment(int id, [FromBody] PaymentReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _paymentService.UpdateAsync(token, id, request);
            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Payment updated successfully."
            });
        }

        // Xóa Payment
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _paymentService.DeleteAsync(token, id);
            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Payment deleted successfully."
            });
        }
    }
}

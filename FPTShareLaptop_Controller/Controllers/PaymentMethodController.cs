using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataAccess.PaymentMethodDTO;
using DataAccess.ResultModel;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        // 🔹 Lấy tất cả phương thức thanh toán
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _paymentMethodService.GetAllAsync();
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Danh sách phương thức thanh toán.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        // 🔹 Lấy phương thức thanh toán theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _paymentMethodService.GetByIdAsync(id);
            if (result == null)
            {
                var errorResponse = new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Không tìm thấy phương thức thanh toán."
                };
                return StatusCode(errorResponse.Code, errorResponse);
            }

            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Thông tin phương thức thanh toán.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        // 🔹 Thêm phương thức thanh toán mới
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentMethodReqModel request)
        {
            await _paymentMethodService.AddAsync(request);
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "Thêm phương thức thanh toán thành công."
            };
            return StatusCode(response.Code, response);
        }

        // 🔹 Cập nhật phương thức thanh toán
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PaymentMethodReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var existingMethod = await _paymentMethodService.GetByIdAsync(id);
            if (existingMethod == null)
            {
                var errorResponse = new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Không tìm thấy phương thức thanh toán."
                };
                return StatusCode(errorResponse.Code, errorResponse);
            }

            await _paymentMethodService.UpdateAsync(token, id, request);
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Cập nhật phương thức thanh toán thành công."
            };
            return StatusCode(response.Code, response);
        }

        // 🔹 Xóa phương thức thanh toán
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var existingMethod = await _paymentMethodService.GetByIdAsync(id);
            if (existingMethod == null)
            {
                var errorResponse = new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Không tìm thấy phương thức thanh toán."
                };
                return StatusCode(errorResponse.Code, errorResponse);
            }

            await _paymentMethodService.DeleteAsync(token, id);
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Xóa phương thức thanh toán thành công."
            };
            return StatusCode(response.Code, response);
        }
    }
}

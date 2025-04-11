using System.Net;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataAccess.ResultModel;
using DataAccess.RefundTransactionDTO;
using System.Threading.Tasks;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundTransactionController : ControllerBase
    {
        private readonly IRefundTransactionService _refundTransactionService;

        public RefundTransactionController(IRefundTransactionService refundTransactionService)
        {
            _refundTransactionService = refundTransactionService;
        }

        // Lấy tất cả giao dịch hoàn tiền
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllRefundTransactions()
        {
            var result = await _refundTransactionService.GetAllAsync();
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Refund transactions retrieved successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        // Lấy giao dịch hoàn tiền theo ID
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetRefundTransactionById(int id)
        {
            var result = await _refundTransactionService.GetByIdAsync(id);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Refund transaction retrieved successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        // Lấy giao dịch hoàn tiền theo WalletId
        [HttpGet]
        [Route("get-by-wallet/{walletId}")]
        public async Task<IActionResult> GetRefundTransactionByWalletId(int walletId)
        {
            var result = await _refundTransactionService.GetByWalletIdAsync(walletId);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Refund transactions retrieved successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        // Thêm mới giao dịch hoàn tiền
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateRefundTransaction([FromBody] RefundTransactionReqModel request)
        {
            await _refundTransactionService.AddAsync(request);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Refund transaction created successfully."
            };
            return StatusCode(response.Code, response);
        }

        // Cập nhật giao dịch hoàn tiền
        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateRefundTransaction([FromBody] RefundTransactionReqModel request, int refundId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _refundTransactionService.UpdateAsync(token, request, refundId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Refund transaction updated successfully."
            };
            return StatusCode(response.Code, response);
        }

        // Xóa giao dịch hoàn tiền
        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteRefundTransaction(int refundId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _refundTransactionService.DeleteAsync(token, refundId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Refund transaction deleted successfully."
            };
            return StatusCode(response.Code, response);
        }
    }
}

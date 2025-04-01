using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Net;
using DataAccess.ResultModel;
using DataAccess.WalletTransaction;

namespace API.Controllers
{
    [Authorize]
    [Route("api/wallet-transactions")]
    [ApiController]
    public class WalletTransactionController : ControllerBase
    {
        private readonly IWalletTransactionService _walletTransactionService;

        public WalletTransactionController(IWalletTransactionService walletTransactionService)
        {
            _walletTransactionService = walletTransactionService;
        }

        // Lấy danh sách giao dịch của user từ JWT
        [HttpGet("list")]
        public async Task<IActionResult> GetTransactionsByUser()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var transactions = await _walletTransactionService.GetTransactionsByUser(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Retrieved transactions successfully.",
                Data = transactions
            };

            return StatusCode(response.Code, response);
        }

        // Lấy giao dịch theo ID
        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            var transaction = await _walletTransactionService.GetTransactionById(transactionId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Transaction retrieved successfully.",
                Data = transaction
            };

            return StatusCode(response.Code, response);
        }

        // Tạo giao dịch mới
        [HttpPost("create")]
        public async Task<IActionResult> CreateTransaction([FromBody] WalletTransactionReqModel model)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var transaction = await _walletTransactionService.CreateTransaction(token, model);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.Created,
                Message = "Transaction created successfully.",
                Data = transaction
            };

            return StatusCode(response.Code, response);
        }

        // Xóa giao dịch
        [HttpDelete("delete/{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _walletTransactionService.DeleteTransaction(token, transactionId);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Transaction deleted successfully."
            };

            return StatusCode(response.Code, response);
        }
    }
}

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataAccess.ResultModel;
using DataAccess.WalletDTO;
using System.Threading.Tasks;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        // Lấy ví theo UserId từ JWT
        [HttpGet]
        [Route("get")]
        public async Task<IActionResult> GetWalletByUser()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _walletService.GetWalletByUser(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Wallet retrived successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }

        // Tạo ví mới
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateWallet([FromBody] WalletReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _walletService.CreateWallet(token, request);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Wallet created successfully."
            };
            return StatusCode(response.Code, response);
        }

        // Nạp tiền vào ví
        [HttpPut]
        [Route("deposit")]
        public async Task<IActionResult> Deposit([FromBody] decimal amount)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _walletService.Deposit(token, amount);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Deposit successfull."
            };
            return StatusCode(response.Code, response);
        }

        // Rút tiền khỏi ví
        [HttpPut]
        [Route("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] decimal amount)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _walletService.Withdraw(token, amount);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Withdraw successfull."
            };
            return StatusCode(response.Code, response);
        }
    }
}

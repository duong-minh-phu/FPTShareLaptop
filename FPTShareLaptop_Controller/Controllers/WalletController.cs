using System.Net;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataAccess.ResultModel;
using DataAccess.WalletDTO;
using System.Threading.Tasks;
using Service.Utils.CustomException;

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

        // Lấy tất cả ví 
        [HttpGet("get")]
        public async Task<IActionResult> GetAllWallets()
        {
            var result = await _walletService.GetAllWallets();
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Wallet retrived successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }


        // Lấy theo ví id
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetWalletById(int id)
        {

            var result = await _walletService.GetWalletById(id);
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
      

        [HttpPost("withdraw-shop")]
        public async Task<IActionResult> WithdrawToShop(int shopId, [FromQuery] decimal amount)
        {

            // Lấy token từ header để xác định người thực hiện (Manager)
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            // Thực hiện chuyển tiền từ Manager đến Shop
            await _walletService.WithdrawFromShopAsync(token, shopId, amount);

            // Log thông báo sau khi chuyển tiền hoàn tất
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Transfer to shop completed successfully."
            };

            return StatusCode(response.Code, response);


        }

    }
}

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

        //Chuyển tiền vào ví Manager sau khi Student thanh toán
        [HttpPost("disburse")]
        public async Task<IActionResult> DisburseToManager([FromQuery] decimal amount)
        {
            await _walletService.DisburseToManagerAsync(amount);
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Disbursement to manager completed successfully."
            };
            return StatusCode(response.Code, response);
        }


        //Manager chuyển tiền cho Shop sau khi trừ phí
        [HttpPost("transfer-to-shop")]
        public async Task<IActionResult> TransferToShop([FromBody] List<ShopTransferReqModel> transfers, [FromQuery] decimal feeRate)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _walletService.TransferFromManagerToShopsAsync(token, transfers, feeRate);
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

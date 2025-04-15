using DataAccess.BorrowContractDTO;
using DataAccess.BorrowRequestDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Net;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowContractController : ControllerBase
    {
        private readonly IBorrowContractService _borrowContractService;

        public BorrowContractController(IBorrowContractService borrowContractService)
        {
            _borrowContractService = borrowContractService;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllBorrowContracts()
        {
            var result = await _borrowContractService.GetAllBorrowContracts();
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = result,
                Message = "Get borrow contract successfully"
            };
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetBorrowContractById(int id)
        {
            var result = await _borrowContractService.GetBorrowContractById(id);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Data = result,
                Message = "Get borrow contract successfully"
            };
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBorrowContract([FromBody] CreateBorrowContractReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _borrowContractService.CreateBorrowContract(token, request);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Borrow contract created successfully",
                Data = result
            };
            return StatusCode(response.Code, response);
        }
      
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBorrowContract(int id, [FromBody] UpdateBorrowContractReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _borrowContractService.UpdateBorrowContract(token, id, request);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Borrow contract updated successfully"
            };
            return StatusCode(response.Code, response);
        }
       
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteBorrowContract(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _borrowContractService.DeleteBorrowContract(token, id);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Borrow contract deleted successfully"
            };
            return StatusCode(response.Code, response);
        }

        [HttpPost("upload-image/{contractId}")]
        [Authorize]
        public async Task<IActionResult> UploadSignedContractImage(int contractId, [FromForm] UploadBorrowContractReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _borrowContractService.UploadSignedContractImage(token, contractId, request);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Signed contract image uploaded successfully."
            };

            return StatusCode(response.Code, response);
        }

    }
}

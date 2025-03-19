using DataAccess.BorrowContractDTO;
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
            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get all borrow contracts successfully",
                Data = result
            });
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetBorrowContractById(int id)
        {
            var result = await _borrowContractService.GetBorrowContractById(id);
            if (result == null)
            {
                return NotFound(new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Borrow contract not found"
                });
            }

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get borrow contract successfully",
                Data = result
            });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBorrowContract([FromBody] CreateBorrowContractReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _borrowContractService.CreateBorrowContract(token, request);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Borrow contract created successfully"
            });
        }
      
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBorrowContract(int id, [FromBody] UpdateBorrowContractReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _borrowContractService.UpdateBorrowContract(token, id, request);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Borrow contract updated successfully"
            });
        }
       
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteBorrowContract(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _borrowContractService.DeleteBorrowContract(token, id);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Borrow contract deleted successfully"
            });
        }
    }
}

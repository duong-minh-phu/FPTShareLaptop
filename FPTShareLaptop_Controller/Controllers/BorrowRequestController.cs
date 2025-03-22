using DataAccess.BorrowRequestDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Net;
using System.Security.Claims;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BorrowRequestController : ControllerBase
    {
        private readonly IBorrowRequestService _borrowRequestService;

        public BorrowRequestController(IBorrowRequestService borrowRequestService)
        {
            _borrowRequestService = borrowRequestService;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllBorrowRequests()
        {
            var result = await _borrowRequestService.GetAllBorrowRequests();

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get all borrow requests successfully",
                Data = result
            });
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetBorrowRequestById(int id)
        {
            var result = await _borrowRequestService.GetBorrowRequestById(id);
            if (result == null)
            {
                return NotFound(new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Borrow request not found"
                });
            }

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get borrow request successfully",
                Data = result
            });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBorrowRequest([FromBody] CreateBorrowRequestReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _borrowRequestService.CreateBorrowRequest(token, request);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Borrow request created successfully"
            });
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBorrowRequest(int id, [FromBody] UpdateBorrowRequestReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _borrowRequestService.UpdateBorrowRequest(token, id, request);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Borrow request updated successfully"
            });
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> SoftDeleteBorrowRequest(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _borrowRequestService.DeleteBorrowRequest(token, id);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Borrow request deleted successfully"
            });
        }
    }
}

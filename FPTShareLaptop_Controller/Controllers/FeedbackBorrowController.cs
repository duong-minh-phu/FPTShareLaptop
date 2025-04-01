using DataAccess.FeedbackBorrowDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackBorrowController : ControllerBase
    {
        private readonly IFeedbackBorrowService _feedbackBorrowService;

        public FeedbackBorrowController(IFeedbackBorrowService feedbackBorrowService)
        {
            _feedbackBorrowService = feedbackBorrowService;
        }

        // Lấy tất cả feedbacks
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllFeedbacks()
        {
            var result = await _feedbackBorrowService.GetAllFeedbacks();
            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get all feedbacks successfully",
                Data = result
            });
        }

        // Lấy feedback theo ID
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetFeedbackById(int id)
        {
            var result = await _feedbackBorrowService.GetFeedbackById(id);
            if (result == null)
            {
                return NotFound(new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Feedback not found"
                });
            }

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get feedback successfully",
                Data = result
            });
        }

        // Tạo feedback mới
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackBorrowReqModel model)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _feedbackBorrowService.CreateFeedback(token, model);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Feedback created successfully"
            });
        }

        // Cập nhật feedback
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, [FromBody] UpdateFeedbackBorrowReqModel model)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _feedbackBorrowService.UpdateFeedback(id, model, token);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Feedback updated successfully"
            });
        }

        // Xóa feedback
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            await _feedbackBorrowService.DeleteFeedback(id);
            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Feedback deleted successfully"
            });
        }
    }
}

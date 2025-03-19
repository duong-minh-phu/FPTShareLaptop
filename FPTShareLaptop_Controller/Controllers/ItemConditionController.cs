using DataAccess.ItemConditionDTO;
using DataAccess.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Net;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemConditionController : ControllerBase
    {
        private readonly IItemConditionService _itemConditionService;

        public ItemConditionController(IItemConditionService itemConditionService)
        {
            _itemConditionService = itemConditionService;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllItemConditions()
        {
            var result = await _itemConditionService.GetAllItemConditions();

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get all item conditions successfully",
                Data = result
            });
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetItemConditionById(int id)
        {
            var result = await _itemConditionService.GetItemConditionById(id);
            if (result == null)
            {
                return NotFound(new ResultModel
                {
                    IsSuccess = false,
                    Code = (int)HttpStatusCode.NotFound,
                    Message = "Item condition not found"
                });
            }

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get item condition successfully",
                Data = result
            });
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateItemCondition([FromBody] CreateItemConditionReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _itemConditionService.CreateItemCondition(token, request);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Item condition created successfully"
            });
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateItemCondition(int id, [FromBody] UpdateItemConditionReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _itemConditionService.UpdateItemCondition(token, id, request);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Item condition updated successfully"
            });
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteItemCondition(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            await _itemConditionService.DeleteItemCondition(token, id);

            return Ok(new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Item condition deleted successfully"
            });
        }
    }
}

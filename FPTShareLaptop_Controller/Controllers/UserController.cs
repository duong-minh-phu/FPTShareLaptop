using DataAccess.ResultModel;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Net;
using System.Threading.Tasks;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Lấy danh sách tất cả người dùng
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Users retrieved successfully.",
                Data = result
            };

            return StatusCode(response.Code, response);
        }

        // Lấy thông tin người dùng theo ID
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            var result = await _userService.GetUserById(userId);
            var response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Users retrieved successfully.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }
    }
}

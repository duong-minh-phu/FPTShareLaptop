using DataAccess.ResultModel;
using DataAccess.StudentDTO;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System.Net;
using System.Threading.Tasks;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("verify-student")]
        public async Task<IActionResult> VerifyStudent([FromForm] StudentReqModel request)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];
            var result = await _studentService.VerifyStudent(request);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Thông tin xác thực thành công.",
                Data = result
            };
            return StatusCode(response.Code, response);
        }
    }
}

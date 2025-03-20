using DataAccess.PasswordDTO;
using DataAccess.RefreshTokenDTO;
using DataAccess.ResultModel;
using DataAccess.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Utils.CustomException;
using System.Net;

namespace FPTShareLaptop_Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("register/student")]
        public async Task<IActionResult> RegisterStudent([FromBody] UserRegisterReqModel studentRegisterReqModel)
        {
           
                await _authenticationService.RegisterStudent(studentRegisterReqModel);
                ResultModel response = new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Student registered successfully."
                };
                return StatusCode(response.Code, response);       
        }

        [HttpPost]
        [Route("register/sponsor")]
        public async Task<IActionResult> RegisterSponsor([FromBody] SponsorRegisterReqModel sponsorRegisterReqModel)
        {           
                await _authenticationService.RegisterSponsor(sponsorRegisterReqModel);
                ResultModel response = new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "Sponsor registered successfully."
                };
                return StatusCode(response.Code, response);
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReqModel userLoginReqModel)
        {
            var result = await _authenticationService.Login(userLoginReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Login successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }
     

        [HttpGet]  
        [Route("user-infor")]
        public async Task<IActionResult> GetUserInfor()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _authenticationService.GetUserInfor(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get user information successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordReqModel forgotPasswordReqModel)
        {
            await _authenticationService.ForgotPassword(forgotPasswordReqModel.Email);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = $"Send OTP code to {forgotPasswordReqModel.Email} successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Authorize]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordReqModel changePasswordReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _authenticationService.ChangePassword(token, changePasswordReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Change password successfully",
            };

            return StatusCode(response.Code, response);
        }    

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(RefreshTokenReqModel refreshTokenReqModel)
        {
            await _authenticationService.Logout(refreshTokenReqModel.RefreshToken);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Logout successfully",
            };

            return StatusCode(response.Code, response);
        }
    }
}

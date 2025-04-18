﻿using DataAccess.PasswordDTO;
using DataAccess.RefreshTokenDTO;
using DataAccess.ResultModel;
using DataAccess.StudentDTO;
using DataAccess.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using Service.Service;
using Service.Utils.CustomException;
using System.Net;
using System.Security.Claims;

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
        public async Task<IActionResult> RegisterStudent([FromForm] StudentRegisterReqModel request)
        {          

            await _authenticationService.RegisterStudent(request);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Student registered successfully."
            };
            return StatusCode(response.Code, response);       
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterReqModel request)
        {           
                await _authenticationService.Register(request);
                ResultModel response = new ResultModel
                {
                    IsSuccess = true,
                    Code = (int)HttpStatusCode.OK,
                    Message = "User registered successfully."
                };
                return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("register/shop")]
        public async Task<IActionResult> RegisterShop([FromForm] ShopRegisterReqModel request)
        {
            await _authenticationService.RegisterShop(request);
            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Shop registered successfully."
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

        [HttpPut]
        [Route("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileReqModel request)
        {
            string token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            await _authenticationService.UpdateUserProfile(token, request);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Profile updated successfully",
            };

            return StatusCode(response.Code, response);
        }


    }
}

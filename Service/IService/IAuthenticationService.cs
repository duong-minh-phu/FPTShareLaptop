﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.PasswordDTO;
using DataAccess.UserDTO;

namespace Service.IService
{
    public interface IAuthenticationService
    {
        Task<UserLoginResModel> Login(UserLoginReqModel userLoginReqModel);
        Task Logout(string refreshToken);
        Task ChangePassword(string token, ChangePasswordReqModel changePasswordReqModel);
        Task ForgotPassword(string email);
        Task<object> GetUserInfor(string token);
        Task<UserRegisterResModel> RegisterStudent(StudentRegisterReqModel studentRegisterReqModel);
        Task<UserRegisterResModel> RegisterSponsor(SponsorRegisterReqModel sponsorRegisterReqModel);
    }
}

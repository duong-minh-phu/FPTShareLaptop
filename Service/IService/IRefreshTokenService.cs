using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.RefreshTokenDTO;

namespace Service.IService
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenResModel> RefreshToken(RefreshTokenReqModel refreshTokenReqModel);
    }
}

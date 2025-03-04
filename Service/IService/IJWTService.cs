using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects.Models;

namespace Service.IService
{
    public interface IJWTService
    {
        Task<string> GenerateJWT(int userId);
        string GenerateRefreshToken();
        string decodeToken(string jwtToken, string nameClaim);
    }

}
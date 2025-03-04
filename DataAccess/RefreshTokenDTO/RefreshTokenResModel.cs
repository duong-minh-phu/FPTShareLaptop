using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.RefreshTokenDTO
{
    public class RefreshTokenResModel
    {
        public string accessToken { get; set; }
        public string newRefreshToken { get; set; }
    }
}

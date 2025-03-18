using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UserDTO
{
    public class UserLoginResModel
    {
        public string Token { get; set; } 
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UserDTO
{
    public class UserLoginResModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string? ProfilePicture { get; set; }
        public string Token { get; set; } // JWT Token
        public string RefreshToken { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}

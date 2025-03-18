using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UserDTO
{
    public class UserRegisterResModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string Message { get; set; }
    }

}

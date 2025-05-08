using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DataAccess.UserDTO
{
    public class UpdateProfileReqModel
    {
        public string? Dob { get; set; } 
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }  
        public IFormFile? AvatarImage { get; set; }
    }
}

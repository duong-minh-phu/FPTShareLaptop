using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UserDTO
{
    public class BaseUserProfileModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string ProfilePicture { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class StudentProfileModel : BaseUserProfileModel
    {
        public string? StudentCode { get; set; }
        public DateOnly? DateOfBirth { get; set; }  
        public DateOnly? EnrollmentDate { get; set; }  
    }

    public class SponsorProfileModel : BaseUserProfileModel
    {
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? Address { get; set; }
    }

}

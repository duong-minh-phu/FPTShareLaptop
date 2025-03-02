using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccess.UserDTO
{
    public class StudentRegisterReqModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleId { get; set; } 
        public string StudentCode { get; set; }
        public string DateOfBirth { get; set; } 
        public string EnrollmentDate { get; set; } 
    }

    public class SponsorRegisterReqModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleId { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Address { get; set; }
    }


}

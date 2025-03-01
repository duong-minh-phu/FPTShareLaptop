using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccess.UserDTO
{
    public class StudentRegisterReqModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; } = "Student"; 
        public string StudentCode { get; set; }
        public string DateOfBirth { get; set; } // Định dạng dd/MM/yyyy
        public string EnrollmentDate { get; set; } // Định dạng dd/MM/yyyy
    }

    public class SponsorRegisterReqModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; } = "Sponsor"; 
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string Address { get; set; }
    }


}

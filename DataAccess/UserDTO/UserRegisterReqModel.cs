using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccess.UserDTO
{
    public class StudentRegisterReqModel
    {
        public string Email { get; set; } = null!;

        public string FullName { get; set; }

        public string Dob { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public string Avatar { get; set; }

        public string Password { get; set; }

        public string StudentCode { get; set; } = null!;

        public string IdentityCard { get; set; } = null!;

        public string EnrollmentDate { get; set; }

    }

    public class UserRegisterReqModel
    {
        public string Email { get; set; } = null!;

        public string FullName { get; set; }

        public string Dob { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }
        public int RoleId {  get; set; }

        public string? Gender { get; set; }

        public string? Avatar { get; set; }

        public string Password { get; set; }
    }


}

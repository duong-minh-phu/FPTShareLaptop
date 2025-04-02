using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace DataAccess.UserDTO
{
    public class StudentRegisterReqModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Date of Birth is required.")]
        public string Dob { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Avatar is required.")]
        public string Avatar { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Student Code is required.")]
        public string StudentCode { get; set; } = null!;
        [Required(ErrorMessage = "Identity Card is required.")]
        public string IdentityCard { get; set; } = null!;
        [Required(ErrorMessage = "Enrollment Date is required.")]
        public string EnrollmentDate { get; set; }
        public IFormFile StudentCardImage { get; set; } = null!;

    }

    public class UserRegisterReqModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "FullName is required.")]
        public string Dob { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required.")]
        public string? PhoneNumber { get; set; }
        public int RoleId {  get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }
        [Required(ErrorMessage = "Avatar is required.")]
        public string? Avatar { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }


}

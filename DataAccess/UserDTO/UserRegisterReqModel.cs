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
        
        [Required(ErrorMessage = "FullName is required.")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Dob is required.")]
        public string Dob { get; set; }
        
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "PhoneNumber is required.")]
        public string PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
       
        [Required(ErrorMessage = "StudentCode is required.")]
        public string StudentCode { get; set; } = null!;
        
        [Required(ErrorMessage = "IdentityCard is required.")]
        public string IdentityCard { get; set; } = null!;
       
        [Required(ErrorMessage = "Enrollment Date is required.")]
        public string EnrollmentDate { get; set; }

        [Required(ErrorMessage = "StudentCardImage is required.")]
        public IFormFile StudentCardImage { get; set; } = null!;

        [Required(ErrorMessage = "AvatarImage is required.")]
        public IFormFile? AvatarImage { get; set; }

    }

    public class UserRegisterReqModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "FullName is required.")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Dob is required.")]
        public string Dob { get; set; }
        
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }
        
        [Required(ErrorMessage = "PhoneNumber is required.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "RoleId is required.")]
        public int RoleId {  get; set; }
        
        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "AvatarImage is required.")]
        public IFormFile? AvatarImage { get; set; }
    }

    public class ShopRegisterReqModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = null!;
        
        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Dob is required.")]
        public string Dob { get; set; }
        
        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }
        
        [Required(ErrorMessage = "PhoneNumber is required.")]
        public string? PhoneNumber { get; set; }
        
        [Required(ErrorMessage = "Gender is required.")]
        public string? Gender { get; set; }
             
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "ShopName is required.")]
        public string ShopName { get; set; } = null!;
        
        [Required(ErrorMessage = "ShopAddress is required.")]
        public string ShopAddress { get; set; } = null!;
        
        [Required(ErrorMessage = "ShopPhone is required.")]
        public string ShopPhone { get; set; } = null!;
        
        [Required(ErrorMessage = "BussinessLicense is required.")]
        public string BusinessLicense { get; set; } = null!;
        
        [Required(ErrorMessage = "BankName is required.")]
        public string BankName { get; set; } = null!;
        
        [Required(ErrorMessage = "BankNumber is required.")]
        public string BankNumber { get; set; } = null!;
        
        [Required(ErrorMessage = "AvatarImage is required.")]
        public IFormFile? AvatarImage { get; set; }
    }

}

﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DataAccess.UserDTO
{
    public class UserRegisterReqModel
    {
        public string Email { get; set; } = null!;

        public string FullName { get; set; }

        public DateTime Dob { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Gender { get; set; }

        public string Avatar { get; set; }

        public string Password { get; set; }

        public string StudentCode { get; set; } = null!;

        public string IdentityCard { get; set; } = null!;

        public DateTime EnrollmentDate { get; set; }

    }

    public class SponsorRegisterReqModel
    {
        public string Email { get; set; } = null!;

        public string FullName { get; set; }

        public DateTime Dob { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Gender { get; set; }

        public string? Avatar { get; set; }

        public string Password { get; set; }
    }


}

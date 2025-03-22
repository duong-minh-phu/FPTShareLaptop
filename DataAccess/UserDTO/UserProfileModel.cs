using System;

namespace DataAccess.UserDTO
{
    public class UserProfileModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Dob { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? Avatar { get; set; }
        public string RoleName { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Thêm trường StudentCode, IdentityCard, EnrollmentDate vào UserProfile luôn
        public string? StudentCode { get; set; }
        public string? IdentityCard { get; set; }
        public string EnrollmentDate { get; set; }
    }
}

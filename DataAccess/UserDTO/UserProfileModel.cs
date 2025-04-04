using System;
using System.Text.Json.Serialization;

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
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? StudentCode { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? IdentityCard { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? EnrollmentDate { get; set; }

        // Thêm trường ShopName, ShopPhone, ShopAddress, BussinessLicense, BankName, BankNumber vào UserProfile luôn
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ShopName { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ShopAddress { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ShopPhone { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BusinessLicense { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BankName { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? BankNumber { get; set; }
    }
}

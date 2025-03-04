using System;

namespace DataAccess.TokenDTO
{
    public class TokenModel
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }

        public TokenModel(int userId, string fullName, string email, string roleName)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
            RoleName = roleName;
        }
    }
}


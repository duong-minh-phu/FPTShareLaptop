﻿using System;

namespace DataAccess.TokenDTO
{
    public class TokenModel
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }

        public TokenModel(string userId, string fullName, string email, string roleName)
        {
            UserId = userId;
            FullName = fullName;
            Email = email;
            RoleName = roleName;
        }
    }
}


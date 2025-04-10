using BusinessObjects.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Service.IService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Service.Service
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly IGenericRepository<User> _userRepository;

        public JWTService(IConfiguration config, IGenericRepository<User> userRepository)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _tokenHandler = new JwtSecurityTokenHandler();
            _userRepository = userRepository;
        }

        public string decodeToken(string jwtToken, string nameClaim)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var claim = token.Claims.FirstOrDefault(c =>
                c.Type == nameClaim || c.Type == ClaimTypes.NameIdentifier);

            return claim?.Value ?? throw new Exception($"Claim '{nameClaim}' not found in token.");
        }



        public async Task<string> GenerateJWT(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId, u => u.Role);
            if (user == null)
                throw new ArgumentException("User not found.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:JwtKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim("userId", user.UserId.ToString()), 
                    new Claim("email", user.Email), 
                    new Claim("role", user.Role.RoleName ?? "User"), 
                    new Claim("fullName", user.FullName)              
                };

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMonths(1),
                signingCredentials: credentials
            );

            return _tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
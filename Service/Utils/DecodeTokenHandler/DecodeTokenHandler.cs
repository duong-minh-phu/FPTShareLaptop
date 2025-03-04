using System.Security.Claims;
using DataAccess.TokenDTO;
using Service.IService;
using Service.Service;

namespace Service.Utils.DecodeTokenHandler
{
    public class DecodeTokenHandler : IDecodeTokenHandler
    {
        private readonly IJWTService _jWTService;

        public DecodeTokenHandler(IJWTService jWTService)
        {
            _jWTService = jWTService;
        }
        public TokenModel decode(string token)
        {
            var roleName = _jWTService.decodeToken(token, ClaimsIdentity.DefaultRoleClaimType);
            var userIdString = _jWTService.decodeToken(token, ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                throw new Exception("Invalid userId format in JWT token");
            }
            var fullName = _jWTService.decodeToken(token, "fullname");
            var email = _jWTService.decodeToken(token, "email");          
            return new TokenModel(userId, email ,fullName, roleName);
        }
    }
}
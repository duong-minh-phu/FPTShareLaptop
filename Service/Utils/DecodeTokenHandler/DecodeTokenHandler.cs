using System.Security.Claims;
using DataAccess.TokenDTO;
using Service.IService;

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
            var roleName = (string) _jWTService.decodeToken(token, ClaimsIdentity.DefaultRoleClaimType);
            var userId = (int) _jWTService.decodeToken(token, "userid");
            var fullName = (string) _jWTService.decodeToken(token, "fullname");
            var email = (string) _jWTService.decodeToken(token, "email");

            return new TokenModel(userId, email ,fullName, roleName);
        }
    }
}
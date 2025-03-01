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
            var roleName = _jWTService.decodeToken(token, ClaimsIdentity.DefaultRoleClaimType);
            var userIdString = _jWTService.decodeToken(token, "userid");
            var fullName = _jWTService.decodeToken(token, "fullname");
            var email = _jWTService.decodeToken(token, "email");
            
            int userId = 0;
            if (!int.TryParse(userIdString, out userId))
            {
                // Nếu userId không hợp lệ, có thể log hoặc xử lý lỗi ở đây
                throw new Exception("Invalid userId format in token.");
            }
            return new TokenModel(userId, email ,fullName, roleName);
        }
    }
}
using BusinessObjects.Models;
using DataAccess.RefreshTokenDTO;
using Service.IService;
using Service.Utils.CustomException;
using System.Net;

namespace Service.Service
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IJWTService _jWTService;

        public RefreshTokenService(
            IGenericRepository<RefreshToken> refreshTokenRepository,
            IGenericRepository<User> userRepository,
            IJWTService jWTService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _jWTService = jWTService;
        }

        public async Task<RefreshTokenResModel> RefreshToken(RefreshTokenReqModel refreshTokenReqModel)
        {
            var currRefreshToken = (await _refreshTokenRepository.GetAllAsync(r => r.Token == refreshTokenReqModel.RefreshToken)).FirstOrDefault();

            if (currRefreshToken == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Refresh token not found");
            }

            if (currRefreshToken.ExpiredAt < DateTime.Now)
            {
                throw new ApiException(HttpStatusCode.BadRequest, "Refresh token is expired");
            }

            // 🔹 Lấy User dựa vào UserId từ RefreshToken
            var user = await _userRepository.GetByIdAsync(currRefreshToken.UserId);
            if (user == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }

            // 🔹 Gọi GenerateJWT với userId thay vì user object
            var token = await _jWTService.GenerateJWT(user.UserId);  

            // 🔹 Tạo Refresh Token mới
            var newRefreshToken = _jWTService.GenerateRefreshToken();
            currRefreshToken.Token = newRefreshToken;
            currRefreshToken.ExpiredAt = DateTime.Now.AddDays(1);

            // 🔹 Cập nhật token mới vào database
            _refreshTokenRepository.Update(currRefreshToken);

            return new RefreshTokenResModel
            {
                accessToken = token,
                newRefreshToken = newRefreshToken
            };
        }
    }
}

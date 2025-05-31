using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.DAL.Interface;

namespace SonitCustom.BLL.Security
{
    public class TokenService : ITokenService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserRepository _userRepository;

        public TokenService(
            IAccessTokenService accessTokenService,
            IRefreshTokenService refreshTokenService,
            IUserRepository userRepository)
        {
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _userRepository = userRepository;
        }

        public AccessTokenDTO GenerateAccessToken(int userId, string role)
        {
            return _accessTokenService.GenerateAccessToken(userId, role);
        }

        public RefreshTokenDTO GenerateRefreshToken(int userId)
        {
            return _refreshTokenService.GenerateRefreshToken(userId);
        }

        public async Task<AccessTokenDTO> RefreshAccessTokenAsync(string refreshToken)
        {
            RefreshTokenDTO? validRefreshToken = await _refreshTokenService.ValidateRefreshTokenAsync(refreshToken);
            if (validRefreshToken == null)
            {
                throw new InvalidRefreshTokenException("Invalid refresh token");
            }

            string? role = await _userRepository.GetRoleByUserIdAsync(validRefreshToken.UserId);
            if (string.IsNullOrEmpty(role))
            {
                throw new Exception("User role not found");
            }

            return _accessTokenService.GenerateAccessToken(validRefreshToken.UserId, role);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
        }

        public async Task RevokeRefreshTokenByUserIdAsync(int userId)
        {
            await _refreshTokenService.RevokeRefreshTokenByUserIdAsync(userId);
        }

        public async Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken)
        {
            return await _refreshTokenService.ValidateRefreshTokenAsync(refreshToken);
        }

        public bool ValidateAccessToken(string accessToken)
        {
            return _accessTokenService.ValidateAccessToken(accessToken);
        }
    }
} 
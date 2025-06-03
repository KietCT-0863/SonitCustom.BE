using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.DAL.Interface;

namespace SonitCustom.BLL.Security
{
    /// <summary>
    /// Service triển khai các thao tác quản lý token xác thực
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Khởi tạo đối tượng TokenService
        /// </summary>
        /// <param name="accessTokenService">Service quản lý access token</param>
        /// <param name="refreshTokenService">Service quản lý refresh token</param>
        /// <param name="userRepository">Repository truy vấn thông tin người dùng</param>
        public TokenService(
            IAccessTokenService accessTokenService,
            IRefreshTokenService refreshTokenService,
            IUserRepository userRepository)
        {
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public AccessTokenDTO GenerateAccessToken(int userId, string role)
        {
            return _accessTokenService.GenerateAccessToken(userId, role);
        }

        /// <inheritdoc />
        public RefreshTokenDTO GenerateRefreshToken(int userId)
        {
            return _refreshTokenService.GenerateRefreshToken(userId);
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            await _refreshTokenService.RevokeRefreshTokenAsync(refreshToken);
        }

        /// <inheritdoc />
        public async Task RevokeRefreshTokenByUserIdAsync(int userId)
        {
            await _refreshTokenService.RevokeRefreshTokenByUserIdAsync(userId);
        }

        /// <inheritdoc />
        public async Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken)
        {
            return await _refreshTokenService.ValidateRefreshTokenAsync(refreshToken);
        }

        /// <inheritdoc />
        public bool ValidateAccessToken(string accessToken)
        {
            return _accessTokenService.ValidateAccessToken(accessToken);
        }
    }
} 
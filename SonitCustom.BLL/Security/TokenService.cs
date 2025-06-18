using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.DAL.Interface;
using System.Text;

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
            // Xác thực refresh token và lấy thông tin người dùng
            RefreshTokenDTO? validatedToken = await ValidateRefreshTokenAsync(refreshToken);
            if (validatedToken == null)
            {
                throw new InvalidRefreshTokenException("Invalid refresh token");
            }

            // Lấy thông tin role của user
            string? role = await _userRepository.GetRoleByUserIdAsync(validatedToken.UserId);
            if (string.IsNullOrEmpty(role))
            {
                throw new Exception("User role not found");
            }

            // Tạo access token mới
            return GenerateAccessToken(validatedToken.UserId, role);
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
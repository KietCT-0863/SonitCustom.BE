using System.Security.Cryptography;
using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.Settings;

namespace SonitCustom.BLL.Security
{
    /// <summary>
    /// Service triển khai các thao tác quản lý refresh token
    /// </summary>
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ITokenStorage _tokenStorage;
        private readonly TokenSettings _tokenSettings;

        /// <summary>
        /// Khởi tạo đối tượng RefreshTokenService
        /// </summary>
        /// <param name="tokenStorage">Service lưu trữ token</param>
        /// <param name="tokenSettings">Cấu hình token</param>
        public RefreshTokenService(ITokenStorage tokenStorage, TokenSettings tokenSettings)
        {
            _tokenStorage = tokenStorage;
            _tokenSettings = tokenSettings;
        }

        /// <inheritdoc />
        public RefreshTokenDTO GenerateRefreshToken(int userId)
        {
            byte[] randomNumber = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            string refreshToken = Convert.ToBase64String(randomNumber);

            RefreshTokenDTO refreshTokenDTO = new RefreshTokenDTO
            {
                Token = refreshToken,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
            };

            _tokenStorage.StoreRefreshToken(refreshTokenDTO);

            return refreshTokenDTO;
        }

        /// <inheritdoc />
        public async Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken)
        {
            RefreshTokenDTO? token = _tokenStorage.GetRefreshToken(refreshToken);
            
            if (token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                return null;
            }

            return token;
        }

        /// <inheritdoc />
        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            if (_tokenStorage.RefreshTokenExists(refreshToken))
            {
                _tokenStorage.RemoveRefreshToken(refreshToken);
            }
        }

        /// <inheritdoc />
        public async Task RevokeRefreshTokenByUserIdAsync(int userId)
        {
            RefreshTokenDTO? token = _tokenStorage.GetRefreshTokenByUserId(userId);
            if (token != null)
            {
                _tokenStorage.RemoveRefreshToken(token.Token);
            }
        }
    }
} 
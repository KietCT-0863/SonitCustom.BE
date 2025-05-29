using System.Security.Cryptography;
using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.Settings;

namespace SonitCustom.BLL.Security
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ITokenStorage _tokenStorage;
        private readonly TokenSettings _tokenSettings;

        public RefreshTokenService(ITokenStorage tokenStorage, TokenSettings tokenSettings)
        {
            _tokenStorage = tokenStorage;
            _tokenSettings = tokenSettings;
        }

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

        public async Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken)
        {
            RefreshTokenDTO? token = _tokenStorage.GetRefreshToken(refreshToken);
            
            if (token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                return null;
            }

            return token;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            if (_tokenStorage.RefreshTokenExists(refreshToken))
            {
                _tokenStorage.RemoveRefreshToken(refreshToken);
            }
        }
    }
} 
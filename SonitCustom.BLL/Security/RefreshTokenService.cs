using System.Security.Cryptography;
using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.Settings;
using System.Text;

namespace SonitCustom.BLL.Security
{
    /// <summary>
    /// Service triển khai các thao tác quản lý refresh token
    /// </summary>
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly TokenSettings _tokenSettings;

        /// <summary>
        /// Khởi tạo đối tượng RefreshTokenService
        /// </summary>
        /// <param name="tokenSettings">Cấu hình token</param>
        public RefreshTokenService(TokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }

        /// <inheritdoc />
        public RefreshTokenDTO GenerateRefreshToken(int userId)
        {
            string randomString = GenerateRandomString();
            string tokenData = $"{userId}:{randomString}";
            string refreshToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenData));

            return new RefreshTokenDTO
            {
                Token = refreshToken,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
            };
        }

        /// <inheritdoc />
        public async Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken)
        {
            try
            {
                int userId = ExtractUserIdFromToken(refreshToken);
                if (userId <= 0)
                {
                    return null;
                }
                
                return new RefreshTokenDTO
                {
                    UserId = userId,
                    Token = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddDays(_tokenSettings.RefreshTokenExpirationDays),
                    CreatedAt = DateTime.UtcNow
                };
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Tạo chuỗi ngẫu nhiên để sử dụng trong token
        /// </summary>
        /// <returns>Chuỗi ngẫu nhiên dưới dạng Base64</returns>
        private string GenerateRandomString()
        {
            byte[] randomNumber = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Trích xuất userId từ refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token cần xử lý</param>
        /// <returns>UserId nếu hợp lệ, 0 nếu không</returns>
        private int ExtractUserIdFromToken(string refreshToken)
        {
            string decodedToken = Encoding.UTF8.GetString(Convert.FromBase64String(refreshToken));
            string[] parts = decodedToken.Split(':');
            
            if (parts.Length < 2 || !int.TryParse(parts[0], out int userId))
            {
                return 0;
            }
            
            return userId;
        }
    }
} 
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.Settings;

namespace SonitCustom.BLL.Security
{
    /// <summary>
    /// Service triển khai các thao tác quản lý access token
    /// </summary>
    public class AccessTokenService : IAccessTokenService
    {
        private readonly TokenSettings _tokenSettings;

        /// <summary>
        /// Khởi tạo đối tượng AccessTokenService
        /// </summary>
        /// <param name="tokenSettings">Cấu hình token</param>
        public AccessTokenService(TokenSettings tokenSettings)
        {
            _tokenSettings = tokenSettings;
        }

        /// <inheritdoc />
        public AccessTokenDTO GenerateAccessToken(int userId, string role)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_tokenSettings.AccessTokenSecret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userid", userId.ToString()),
                    new Claim("role", role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_tokenSettings.AccessTokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return new AccessTokenDTO
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresAt = tokenDescriptor.Expires.Value,
                UserId = userId,
                Role = role
            };
        }

        /// <inheritdoc />
        public bool ValidateAccessToken(string accessToken)
        {
            try
            {
                GetPrincipalFromAccessToken(accessToken);
                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public ClaimsPrincipal? GetPrincipalFromAccessToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSettings.AccessTokenSecret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out _);
            return principal;
        }
    }
} 
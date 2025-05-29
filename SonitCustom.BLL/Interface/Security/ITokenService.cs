using SonitCustom.BLL.DTOs.Auth;

namespace SonitCustom.BLL.Interface.Security
{
    public interface ITokenService
    {
        AccessTokenDTO GenerateAccessToken(int userId, string role);
        RefreshTokenDTO GenerateRefreshToken(int userId);
        Task<AccessTokenDTO> RefreshAccessTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken);
        bool ValidateAccessToken(string accessToken);
    }
} 
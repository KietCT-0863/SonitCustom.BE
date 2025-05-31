using SonitCustom.BLL.DTOs.Auth;

namespace SonitCustom.BLL.Interface.Security
{
    public interface IRefreshTokenService
    {
        RefreshTokenDTO GenerateRefreshToken(int userId);
        Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenByUserIdAsync(int userId);
    }
}
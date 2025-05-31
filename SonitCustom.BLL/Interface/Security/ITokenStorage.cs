using SonitCustom.BLL.DTOs.Auth;

namespace SonitCustom.BLL.Interface.Security
{
    public interface ITokenStorage
    {
        void StoreRefreshToken(RefreshTokenDTO refreshToken);
        RefreshTokenDTO GetRefreshToken(string token);
        RefreshTokenDTO GetRefreshTokenByUserId(int userId);
        void RemoveRefreshToken(string token);
        bool RefreshTokenExists(string token);
    }
} 
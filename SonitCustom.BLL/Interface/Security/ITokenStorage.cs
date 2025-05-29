using SonitCustom.BLL.DTOs.Auth;

namespace SonitCustom.BLL.Interface.Security
{
    public interface ITokenStorage
    {
        void StoreRefreshToken(RefreshTokenDTO refreshToken);
        RefreshTokenDTO GetRefreshToken(string token);
        void RemoveRefreshToken(string token);
        bool RefreshTokenExists(string token);
    }
} 
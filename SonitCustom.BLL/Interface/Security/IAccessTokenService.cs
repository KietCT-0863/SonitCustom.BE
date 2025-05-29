using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.DTOs.Auth;
using System.Security.Claims;

namespace SonitCustom.BLL.Interface.Security
{
    public interface IAccessTokenService
    {
        AccessTokenDTO GenerateAccessToken(int userId, string role);
        bool ValidateAccessToken(string accessToken);
        ClaimsPrincipal? GetPrincipalFromAccessToken(string accessToken);
    }
} 
using SonitCustom.BLL.DTOs.Auth;
using System.Security.Claims;

namespace SonitCustom.BLL.Interface.Security
{
    /// <summary>
    /// Service quản lý các thao tác liên quan đến access token
    /// </summary>
    public interface IAccessTokenService
    {
        /// <summary>
        /// Tạo mới access token
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <param name="role">Vai trò của người dùng</param>
        /// <returns>Đối tượng <see cref="AccessTokenDTO"/> chứa token và thông tin liên quan</returns>
        AccessTokenDTO GenerateAccessToken(int userId, string role);
        
        /// <summary>
        /// Xác thực access token
        /// </summary>
        /// <param name="accessToken">Access token cần xác thực</param>
        /// <returns>True nếu token hợp lệ, ngược lại False</returns>
        bool ValidateAccessToken(string accessToken);

        /// <summary>
        /// Lấy thông tin principal từ access token
        /// </summary>
        /// <param name="accessToken">Access token cần xử lý</param>
        /// <returns>Đối tượng <see cref="ClaimsPrincipal"/> chứa các claims, hoặc null nếu token không hợp lệ</returns>
        /// <exception cref="SecurityTokenExpiredException">Khi token đã hết hạn</exception>
        ClaimsPrincipal? GetPrincipalFromAccessToken(string accessToken);
    }
} 
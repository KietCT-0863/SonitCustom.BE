using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.BLL.Interface.Security
{
    /// <summary>
    /// Service quản lý các thao tác liên quan đến token xác thực
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Tạo mới access token
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <param name="role">Vai trò của người dùng</param>
        /// <returns>Đối tượng <see cref="AccessTokenDTO"/> chứa token và thông tin liên quan</returns>
        AccessTokenDTO GenerateAccessToken(int userId, string role);
        
        /// <summary>
        /// Tạo mới refresh token
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Đối tượng <see cref="RefreshTokenDTO"/> chứa token và thông tin liên quan</returns>
        RefreshTokenDTO GenerateRefreshToken(int userId);
        
        /// <summary>
        /// Làm mới access token từ refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token cần xác thực</param>
        /// <returns>Đối tượng <see cref="AccessTokenDTO"/> mới</returns>
        /// <exception cref="InvalidRefreshTokenException">Ném ra khi refresh token không hợp lệ</exception>
        /// <exception cref="Exception">Ném ra khi không tìm thấy role của người dùng</exception>
        Task<AccessTokenDTO> RefreshAccessTokenAsync(string refreshToken);
        
        /// <summary>
        /// Thu hồi refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token cần thu hồi</param>
        Task RevokeRefreshTokenAsync(string refreshToken);
        
        /// <summary>
        /// Thu hồi tất cả refresh token của người dùng
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        Task RevokeRefreshTokenByUserIdAsync(int userId);
        
        /// <summary>
        /// Xác thực refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token cần xác thực</param>
        /// <returns>Đối tượng <see cref="RefreshTokenDTO"/> nếu hợp lệ, null nếu không</returns>
        Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken);
        
        /// <summary>
        /// Xác thực access token
        /// </summary>
        /// <param name="accessToken">Access token cần xác thực</param>
        /// <returns>True nếu token hợp lệ, ngược lại False</returns>
        bool ValidateAccessToken(string accessToken);
    }
} 
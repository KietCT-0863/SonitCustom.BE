using SonitCustom.BLL.DTOs.Auth;

namespace SonitCustom.BLL.Interface.Security
{
    /// <summary>
    /// Service quản lý các thao tác liên quan đến refresh token
    /// </summary>
    public interface IRefreshTokenService
    {
        /// <summary>
        /// Tạo mới refresh token
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Đối tượng <see cref="RefreshTokenDTO"/> chứa token và thông tin liên quan</returns>
        RefreshTokenDTO GenerateRefreshToken(int userId);
        
        /// <summary>
        /// Xác thực refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token cần xác thực</param>
        /// <returns>Đối tượng <see cref="RefreshTokenDTO"/> nếu hợp lệ, null nếu hết hạn hoặc không tồn tại</returns>
        Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken);
    }
}
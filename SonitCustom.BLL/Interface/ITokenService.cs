using SonitCustom.BLL.DTOs;

namespace SonitCustom.BLL.Interface
{
    public interface ITokenService
    {
        /// <summary>
        /// Tạo access token mới cho user
        /// </summary>
        /// <param name="userId">ID của user</param>
        /// <param name="role">Vai trò của user</param>
        /// <returns>Access token mới</returns>
        AccessTokenDTO GenerateAccessToken(int userId, string role);

        /// <summary>
        /// Tạo refresh token mới cho user
        /// </summary>
        /// <param name="userId">ID của user</param>
        /// <returns>Refresh token mới</returns>
        RefreshTokenDTO GenerateRefreshToken(int userId);

        /// <summary>
        /// Làm mới access token bằng refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token hiện tại</param>
        /// <returns>Access token mới</returns>
        Task<AccessTokenDTO> RefreshAccessTokenAsync(string refreshToken);

        /// <summary>
        /// Hủy một refresh token
        /// </summary>
        /// <param name="refreshToken">Refresh token cần hủy</param>
        Task RevokeRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Kiểm tra tính hợp lệ của refresh token và trả về thông tin token
        /// </summary>
        /// <param name="refreshToken">Refresh token cần kiểm tra</param>
        /// <returns>Thông tin refresh token nếu hợp lệ, null nếu không hợp lệ</returns>
        Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Kiểm tra tính hiệu lực của access token
        /// </summary>
        /// <param name="accessToken">Access token cần kiểm tra</param>
        /// <returns>True nếu token còn hiệu lực, False nếu token đã hết hạn</returns>
        bool ValidateAccessToken(string accessToken);
    }
} 
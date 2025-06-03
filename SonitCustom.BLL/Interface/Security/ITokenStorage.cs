using SonitCustom.BLL.DTOs.Auth;

namespace SonitCustom.BLL.Interface.Security
{
    /// <summary>
    /// Service quản lý việc lưu trữ và truy xuất refresh token
    /// </summary>
    public interface ITokenStorage
    {
        /// <summary>
        /// Lưu trữ refresh token
        /// </summary>
        /// <param name="refreshToken">Đối tượng <see cref="RefreshTokenDTO"/> cần lưu trữ</param>
        /// <remarks>Nếu đã tồn tại token cho user này, token cũ sẽ bị thay thế</remarks>
        void StoreRefreshToken(RefreshTokenDTO refreshToken);
        
        /// <summary>
        /// Lấy refresh token theo giá trị token
        /// </summary>
        /// <param name="token">Giá trị token cần tìm</param>
        /// <returns>Đối tượng <see cref="RefreshTokenDTO"/> nếu tìm thấy, ngược lại null</returns>
        RefreshTokenDTO GetRefreshToken(string token);
        
        /// <summary>
        /// Lấy refresh token theo ID người dùng
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Đối tượng <see cref="RefreshTokenDTO"/> nếu tìm thấy, ngược lại null</returns>
        RefreshTokenDTO GetRefreshTokenByUserId(int userId);
        
        /// <summary>
        /// Xóa refresh token
        /// </summary>
        /// <param name="token">Giá trị token cần xóa</param>
        void RemoveRefreshToken(string token);
        
        /// <summary>
        /// Kiểm tra refresh token có tồn tại hay không
        /// </summary>
        /// <param name="token">Giá trị token cần kiểm tra</param>
        /// <returns>True nếu tồn tại, ngược lại False</returns>
        bool RefreshTokenExists(string token);
    }
} 
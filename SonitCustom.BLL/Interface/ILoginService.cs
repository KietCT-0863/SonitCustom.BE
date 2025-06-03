using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.BLL.Interface
{
    /// <summary>
    /// Service xử lý các thao tác đăng nhập hệ thống
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Xác thực người dùng dựa trên tên đăng nhập và mật khẩu
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>Đối tượng <see cref="UserDTO"/> chứa thông tin người dùng nếu xác thực thành công</returns>
        /// <exception cref="InvalidCredentialsException">Ném ra khi thông tin đăng nhập không hợp lệ</exception>
        Task<UserDTO> LoginAsync(string username, string password);
    }
} 
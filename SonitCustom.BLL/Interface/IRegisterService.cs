using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.BLL.Interface
{
    /// <summary>
    /// Service xử lý các thao tác đăng ký người dùng mới
    /// </summary>
    public interface IRegisterService
    {
        /// <summary>
        /// Đăng ký tài khoản người dùng mới
        /// </summary>
        /// <param name="newUser">Đối tượng <see cref="RegisterUserDTO"/> chứa thông tin người dùng cần đăng ký</param>
        /// <returns>True nếu đăng ký thành công</returns>
        /// <exception cref="UserCredentialsAlreadyExistsException">Ném ra khi tên đăng nhập hoặc email đã tồn tại</exception>
        Task<bool> RegisterAsync(RegisterUserDTO newUser);
    }
}
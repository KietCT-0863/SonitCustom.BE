using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Exceptions;
using SonitCustom.DAL.Interface;

namespace SonitCustom.BLL.Services
{
    /// <summary>
    /// Service triển khai các thao tác đăng nhập hệ thống
    /// </summary>
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Khởi tạo đối tượng LoginService
        /// </summary>
        /// <param name="userRepository">Repository truy vấn dữ liệu user</param>
        public LoginService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<UserDTO> LoginAsync(string username, string password)
        {
            User? user = await GetValidUserAsync(username, password);

            if (user == null)
            {
                throw new InvalidCredentialsException(username);
            }

            return MapUserToDto(user);
        }

        /// <summary>
        /// Xác thực thông tin đăng nhập và lấy thông tin user
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>Đối tượng User nếu thông tin hợp lệ, ngược lại null</returns>
        private async Task<User?> GetValidUserAsync(string username, string password)
        {
            return await _userRepository.GetUserAsync(username, password);
        }

        /// <summary>
        /// Chuyển đổi đối tượng User thành UserDTO
        /// </summary>
        /// <param name="user">Đối tượng User cần chuyển đổi</param>
        /// <returns>Đối tượng UserDTO</returns>
        private UserDTO MapUserToDto(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Fullname = user.Fullname,
                RoleName = user.RoleNavigation.RoleName
            };
        }
    }
}
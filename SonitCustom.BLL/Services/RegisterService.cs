using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;

namespace SonitCustom.BLL.Services
{
    /// <summary>
    /// Service triển khai các thao tác đăng ký người dùng mới
    /// </summary>
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;
        private const int DEFAULT_USER_ROLE = 2;

        /// <summary>
        /// Khởi tạo đối tượng RegisterService
        /// </summary>
        /// <param name="userRepository">Repository truy vấn dữ liệu user</param>
        public RegisterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<bool> RegisterAsync(RegisterUserDTO newRegister)
        {
            await CheckUserExistenceAsync(newRegister.Username, newRegister.Email);

            User newUser = MapDtoToUserEntity(newRegister);
            
            await _userRepository.AddNewUserAsync(newUser);
            return true;
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của tên đăng nhập hoặc email
        /// </summary>
        /// <param name="username">Tên đăng nhập cần kiểm tra</param>
        /// <param name="email">Email cần kiểm tra</param>
        /// <exception cref="UserCredentialsAlreadyExistsException">Ném ra khi tên đăng nhập hoặc email đã tồn tại</exception>
        private async Task CheckUserExistenceAsync(string username, string email)
        {
            bool userExists = await _userRepository.CheckUserExistsAsync(username, email);
            
            if (userExists)
            {
                throw new UserCredentialsAlreadyExistsException(username, email);
            }
        }

        /// <summary>
        /// Chuyển đổi đối tượng RegisterUserDTO thành User
        /// </summary>
        /// <param name="dto">Đối tượng RegisterUserDTO cần chuyển đổi</param>
        /// <returns>Đối tượng User</returns>
        private User MapDtoToUserEntity(RegisterUserDTO dto)
        {
            return new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Fullname = dto.Fullname,
                Email = dto.Email,
                Role = DEFAULT_USER_ROLE
            };
        }
    }
}
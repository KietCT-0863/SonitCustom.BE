using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.BLL.Services
{
    /// <summary>
    /// Service triển khai các thao tác liên quan đến user
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        /// <summary>
        /// Khởi tạo đối tượng UserService
        /// </summary>
        /// <param name="userRepository">Repository truy vấn dữ liệu user</param>
        /// <param name="roleRepository">Repository truy vấn dữ liệu role</param>
        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        /// <inheritdoc />
        public async Task<RespondUserDTO> GetUserByIdAsync(int id)
        {
            User? user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException(id);
            }

            return MapUserToRespondDto(user);
        }

        /// <inheritdoc />
        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            List<User> users = await _userRepository.GetAllUserAsync();
            return users.Select(MapUserToDto).ToList();
        }

        /// <inheritdoc />
        public async Task<string?> GetUserRoleAsync(int userId)
        {
            return await _userRepository.GetRoleByUserIdAsync(userId);
        }

        /// <inheritdoc />
        public async Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO)
        {
            User? userToUpdate = await GetUserByIdOrThrowAsync(userId);

            await ValidateUserInfoAsync(userId, updateUserDTO.Username, updateUserDTO.Email, userToUpdate.Username, userToUpdate.Email);

            UpdateUserFields(userToUpdate, updateUserDTO);

            await _userRepository.UpdateUserAsync(userToUpdate);
        }

        /// <inheritdoc />
        public async Task AdminUpdateUserAsync(int userId, AdminUpdateUserDTO adminUpdateUserDTO)
        {
            User? userToUpdate = await GetUserByIdOrThrowAsync(userId);

            await ValidateUserInfoAsync(userId, adminUpdateUserDTO.Username, adminUpdateUserDTO.Email, userToUpdate.Username, userToUpdate.Email);

            if (adminUpdateUserDTO.Role.HasValue)
            {
                await ValidateRoleAsync(adminUpdateUserDTO.Role.Value);
            }

            UpdateUserFields(userToUpdate, adminUpdateUserDTO);

            await _userRepository.UpdateUserAsync(userToUpdate);
        }

        /// <inheritdoc />
        public async Task DeleteAccountAsync(int userId)
        {
            User user = await GetUserByIdOrThrowAsync(userId);
            
            string? userRole = user.RoleNavigation?.RoleName;
            if (userRole?.ToLower() == "admin")
            {
                throw new AdminDeleteException(userId);
            }
            
            await _userRepository.DeleteUserAsync(user);
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
                Fullname = user.Fullname,
                Email = user.Email,
                RoleName = user.RoleNavigation.RoleName
            };
        }

        /// <summary>
        /// Chuyển đổi đối tượng User thành RespondUserDTO
        /// </summary>
        /// <param name="user">Đối tượng User cần chuyển đổi</param>
        /// <returns>Đối tượng RespondUserDTO</returns>
        private RespondUserDTO MapUserToRespondDto(User user)
        {
            return new RespondUserDTO
            {
                Username = user.Username,
                Fullname = user.Fullname,
                Email = user.Email,
                Password = user.Password,
                Rolename = user.RoleNavigation.RoleName
            };
        }

        /// <summary>
        /// Lấy đối tượng User theo ID hoặc ném ra ngoại lệ nếu không tìm thấy
        /// </summary>
        /// <param name="userId">ID của user cần lấy</param>
        /// <returns>Đối tượng User</returns>
        /// <exception cref="UserNotFoundException">Ném ra khi không tìm thấy user</exception>
        private async Task<User> GetUserByIdOrThrowAsync(int userId)
        {
            User? user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            return user;
        }

        /// <summary>
        /// Xác thực thông tin user khi cập nhật
        /// </summary>
        /// <param name="userId">ID của user cần cập nhật</param>
        /// <param name="newUsername">Tên đăng nhập mới</param>
        /// <param name="newEmail">Email mới</param>
        /// <param name="currentUsername">Tên đăng nhập hiện tại</param>
        /// <param name="currentEmail">Email hiện tại</param>
        private async Task ValidateUserInfoAsync(int userId, string? newUsername, string? newEmail, string currentUsername, string currentEmail)
        {
            await ValidateUsernameAsync(userId, newUsername, currentUsername);
            await ValidateEmailAsync(userId, newEmail, currentEmail);
        }

        /// <summary>
        /// Xác thực tính duy nhất của tên đăng nhập
        /// </summary>
        /// <param name="userId">ID của user cần cập nhật</param>
        /// <param name="newUsername">Tên đăng nhập mới</param>
        /// <param name="currentUsername">Tên đăng nhập hiện tại</param>
        /// <exception cref="DuplicateUserNameException">Ném ra khi tên đăng nhập đã tồn tại</exception>
        private async Task ValidateUsernameAsync(int userId, string? newUsername, string currentUsername)
        {
            if (!string.IsNullOrEmpty(newUsername) && newUsername != currentUsername &&
                await _userRepository.IsUserNameExistsAsync(newUsername, userId))
            {
                throw new DuplicateUserNameException(newUsername);
            }
        }

        /// <summary>
        /// Xác thực tính duy nhất của email
        /// </summary>
        /// <param name="userId">ID của user cần cập nhật</param>
        /// <param name="newEmail">Email mới</param>
        /// <param name="currentEmail">Email hiện tại</param>
        /// <exception cref="DuplicateEmailException">Ném ra khi email đã tồn tại</exception>
        private async Task ValidateEmailAsync(int userId, string? newEmail, string currentEmail)
        {
            if (!string.IsNullOrEmpty(newEmail) && newEmail != currentEmail &&
                await _userRepository.IsEmailExistsAsync(newEmail, userId))
            {
                throw new DuplicateEmailException(newEmail);
            }
        }

        /// <summary>
        /// Xác thực sự tồn tại của role
        /// </summary>
        /// <param name="roleId">ID của role cần xác thực</param>
        /// <exception cref="RoleNotFoundException">Ném ra khi không tìm thấy role</exception>
        private async Task ValidateRoleAsync(int roleId)
        {
            if (!await _roleRepository.IsRoleExistsAsync(roleId))
            {
                throw new RoleNotFoundException(roleId);
            }
        }

        /// <summary>
        /// Cập nhật thông tin user từ UpdateUserDTO
        /// </summary>
        /// <param name="user">Đối tượng User cần cập nhật</param>
        /// <param name="updateDto">Dữ liệu cập nhật</param>
        private void UpdateUserFields(User user, UpdateUserDTO updateDto)
        {
            user.Username = string.IsNullOrEmpty(updateDto.Username) ? user.Username : updateDto.Username;
            user.Password = string.IsNullOrEmpty(updateDto.Password) ? user.Password : updateDto.Password;
            user.Email = string.IsNullOrEmpty(updateDto.Email) ? user.Email : updateDto.Email;
            user.Fullname = string.IsNullOrEmpty(updateDto.Fullname) ? user.Fullname : updateDto.Fullname;
        }

        /// <summary>
        /// Cập nhật thông tin user từ AdminUpdateUserDTO (bao gồm cả role)
        /// </summary>
        /// <param name="user">Đối tượng User cần cập nhật</param>
        /// <param name="adminUpdateDto">Dữ liệu cập nhật kèm role</param>
        private void UpdateUserFields(User user, AdminUpdateUserDTO adminUpdateDto)
        {
            user.Username = string.IsNullOrEmpty(adminUpdateDto.Username) ? user.Username : adminUpdateDto.Username;
            user.Password = string.IsNullOrEmpty(adminUpdateDto.Password) ? user.Password : adminUpdateDto.Password;
            user.Email = string.IsNullOrEmpty(adminUpdateDto.Email) ? user.Email : adminUpdateDto.Email;
            user.Fullname = string.IsNullOrEmpty(adminUpdateDto.Fullname) ? user.Fullname : adminUpdateDto.Fullname;

            if (adminUpdateDto.Role.HasValue)
            {
                user.Role = adminUpdateDto.Role.Value;
            }
        }
    }
}
using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            User? user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException(id);
            }

            return MapUserToDto(user);
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            List<User> users = await _userRepository.GetAllUserAsync();
            return users.Select(MapUserToDto).ToList();
        }

        public async Task<string?> GetUserRoleAsync(int userId)
        {
            return await _userRepository.GetRoleByUserIdAsync(userId);
        }

        public async Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO)
        {
            User? userToUpdate = await GetUserByIdOrThrowAsync(userId);

            await ValidateUserInfoAsync(userId, updateUserDTO.Username, updateUserDTO.Email, userToUpdate.username, userToUpdate.email);

            UpdateUserFields(userToUpdate, updateUserDTO);

            await _userRepository.UpdateUserAsync(userToUpdate);
        }

        public async Task AdminUpdateUserAsync(int userId, AdminUpdateUserDTO adminUpdateUserDTO)
        {
            User? userToUpdate = await GetUserByIdOrThrowAsync(userId);

            await ValidateUserInfoAsync(userId, adminUpdateUserDTO.Username, adminUpdateUserDTO.Email, userToUpdate.username, userToUpdate.email);

            if (adminUpdateUserDTO.Role.HasValue)
            {
                await ValidateRoleAsync(adminUpdateUserDTO.Role.Value);
            }

            UpdateUserFields(userToUpdate, adminUpdateUserDTO);

            await _userRepository.UpdateUserAsync(userToUpdate);
        }

        private UserDTO MapUserToDto(User user)
        {
            return new UserDTO
            {
                Id = user.id,
                Username = user.username,
                Fullname = user.fullname,
                Email = user.email,
                RoleName = user.roleNavigation.roleName
            };
        }

        private async Task<User> GetUserByIdOrThrowAsync(int userId)
        {
            User? user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                throw new UserNotFoundException(userId);
            }

            return user;
        }

        private async Task ValidateUserInfoAsync(int userId, string? newUsername, string? newEmail, string currentUsername, string currentEmail)
        {
            await ValidateUsernameAsync(userId, newUsername, currentUsername);
            await ValidateEmailAsync(userId, newEmail, currentEmail);
        }

        private async Task ValidateUsernameAsync(int userId, string? newUsername, string currentUsername)
        {
            if (!string.IsNullOrEmpty(newUsername) && newUsername != currentUsername &&
                await _userRepository.IsUserNameExistsAsync(newUsername, userId))
            {
                throw new DuplicateUserNameException(newUsername);
            }
        }

        private async Task ValidateEmailAsync(int userId, string? newEmail, string currentEmail)
        {
            if (!string.IsNullOrEmpty(newEmail) && newEmail != currentEmail &&
                await _userRepository.IsEmailExistsAsync(newEmail, userId))
            {
                throw new DuplicateEmailException(newEmail);
            }
        }

        private async Task ValidateRoleAsync(int roleId)
        {
            if (!await _roleRepository.IsRoleExistsAsync(roleId))
            {
                throw new RoleNotFoundException(roleId);
            }
        }

        private void UpdateUserFields(User user, UpdateUserDTO updateDto)
        {
            user.username = string.IsNullOrEmpty(updateDto.Username) ? user.username : updateDto.Username;
            user.password = string.IsNullOrEmpty(updateDto.Password) ? user.password : updateDto.Password;
            user.email = string.IsNullOrEmpty(updateDto.Email) ? user.email : updateDto.Email;
            user.fullname = string.IsNullOrEmpty(updateDto.Fullname) ? user.fullname : updateDto.Fullname;
        }

        private void UpdateUserFields(User user, AdminUpdateUserDTO adminUpdateDto)
        {
            user.username = string.IsNullOrEmpty(adminUpdateDto.Username) ? user.username : adminUpdateDto.Username;
            user.password = string.IsNullOrEmpty(adminUpdateDto.Password) ? user.password : adminUpdateDto.Password;
            user.email = string.IsNullOrEmpty(adminUpdateDto.Email) ? user.email : adminUpdateDto.Email;
            user.fullname = string.IsNullOrEmpty(adminUpdateDto.Fullname) ? user.fullname : adminUpdateDto.Fullname;

            if (adminUpdateDto.Role.HasValue)
            {
                user.role = adminUpdateDto.Role.Value;
            }
        }
    }
}
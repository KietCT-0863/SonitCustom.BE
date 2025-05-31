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

        public async Task<RespondUserDTO> GetUserByIdAsync(int id)
        {
            User? user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                throw new UserNotFoundException(id);
            }

            return MapUserToRespondDto(user);
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

            await ValidateUserInfoAsync(userId, updateUserDTO.Username, updateUserDTO.Email, userToUpdate.Username, userToUpdate.Email);

            UpdateUserFields(userToUpdate, updateUserDTO);

            await _userRepository.UpdateUserAsync(userToUpdate);
        }

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
            user.Username = string.IsNullOrEmpty(updateDto.Username) ? user.Username : updateDto.Username;
            user.Password = string.IsNullOrEmpty(updateDto.Password) ? user.Password : updateDto.Password;
            user.Email = string.IsNullOrEmpty(updateDto.Email) ? user.Email : updateDto.Email;
            user.Fullname = string.IsNullOrEmpty(updateDto.Fullname) ? user.Fullname : updateDto.Fullname;
        }

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
using SonitCustom.BLL.DTOs.Users;

namespace SonitCustom.BLL.Interface
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<string?> GetUserRoleAsync(int userId);
        Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO);
        Task AdminUpdateUserAsync(int userId, AdminUpdateUserDTO adminUpdateUserDTO);
    }
} 
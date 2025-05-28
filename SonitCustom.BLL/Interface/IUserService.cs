using SonitCustom.BLL.DTOs;

namespace SonitCustom.BLL.Interface
{
    public interface IUserService
    {
        /// <summary>
        /// Lấy thông tin user theo ID
        /// </summary>
        /// <param name="id">ID của user</param>
        /// <returns>Thông tin user</returns>
        Task<UserDTO> GetUserByIdAsync(int id);

        /// <summary>
        /// Lấy danh sách tất cả user
        /// </summary>
        /// <returns>Danh sách user</returns>
        Task<List<UserDTO>> GetAllUsersAsync();

        /// <summary>
        /// Lấy role của user theo ID
        /// </summary>
        /// <param name="userId">ID của user</param>
        /// <returns>Role của user</returns>
        Task<string?> GetUserRoleAsync(int userId);
    }
} 
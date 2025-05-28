using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Lấy danh sách tất cả user
        /// </summary>
        /// <returns>Danh sách user</returns>
        Task<List<User>> GetAllUserAsync();

        /// <summary>
        /// Thêm user mới
        /// </summary>
        /// <param name="newUser">Thông tin user mới</param>
        Task AddNewUserAsync(User newUser);

        /// <summary>
        /// Cập nhật thông tin user
        /// </summary>
        /// <param name="userToUpdate">Thông tin user cần cập nhật</param>
        Task UpdateUserAsync(User userToUpdate);

        /// <summary>
        /// Lấy thông tin user theo username và password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns>Thông tin user</returns>
        Task<User?> GetUserAsync(string username, string password);

        /// <summary>
        /// Lấy role của user theo ID
        /// </summary>
        /// <param name="userId">ID của user</param>
        /// <returns>Role của user</returns>
        Task<string?> GetRoleByUserIdAsync(int userId);
    }
}
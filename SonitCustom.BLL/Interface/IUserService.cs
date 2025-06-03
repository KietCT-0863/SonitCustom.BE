using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.BLL.Interface
{
    /// <summary>
    /// Service xử lý các thao tác liên quan đến user
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Lấy thông tin chi tiết user theo ID
        /// </summary>
        /// <param name="id">ID của user cần lấy thông tin</param>
        /// <returns>Đối tượng <see cref="RespondUserDTO"/> chứa thông tin chi tiết của user</returns>
        /// <exception cref="UserNotFoundException">Ném ra khi không tìm thấy user</exception>
        Task<RespondUserDTO> GetUserByIdAsync(int id);

        /// <summary>
        /// Lấy danh sách tất cả user
        /// </summary>
        /// <returns>Danh sách các đối tượng <see cref="UserDTO"/></returns>
        Task<List<UserDTO>> GetAllUsersAsync();

        /// <summary>
        /// Lấy role của user theo ID
        /// </summary>
        /// <param name="userId">ID của user cần lấy role</param>
        /// <returns>Tên role của user hoặc null nếu không tìm thấy</returns>
        Task<string?> GetUserRoleAsync(int userId);

        /// <summary>
        /// Cập nhật thông tin user bởi chính user đó
        /// </summary>
        /// <param name="userId">ID của user cần cập nhật</param>
        /// <param name="updateUserDTO">Đối tượng <see cref="UpdateUserDTO"/> chứa thông tin cập nhật</param>
        /// <exception cref="UserNotFoundException">Ném ra khi không tìm thấy user</exception>
        /// <exception cref="DuplicateUserNameException">Ném ra khi tên đăng nhập đã tồn tại</exception>
        /// <exception cref="DuplicateEmailException">Ném ra khi email đã tồn tại</exception>
        Task UpdateUserAsync(int userId, UpdateUserDTO updateUserDTO);

        /// <summary>
        /// Cập nhật thông tin user bởi admin (bao gồm cả role)
        /// </summary>
        /// <param name="userId">ID của user cần cập nhật</param>
        /// <param name="adminUpdateUserDTO">Đối tượng <see cref="AdminUpdateUserDTO"/> chứa thông tin cập nhật kèm role</param>
        /// <exception cref="UserNotFoundException">Ném ra khi không tìm thấy user</exception>
        /// <exception cref="DuplicateUserNameException">Ném ra khi tên đăng nhập đã tồn tại</exception>
        /// <exception cref="DuplicateEmailException">Ném ra khi email đã tồn tại</exception>
        /// <exception cref="RoleNotFoundException">Ném ra khi không tìm thấy role được chỉ định</exception>
        Task AdminUpdateUserAsync(int userId, AdminUpdateUserDTO adminUpdateUserDTO);

        /// <summary>
        /// Xóa tài khoản user
        /// </summary>
        /// <param name="userId">ID của user cần xóa</param>
        /// <exception cref="UserNotFoundException">Ném ra khi không tìm thấy user</exception>
        /// <exception cref="AdminDeleteException">Ném ra khi cố gắng xóa tài khoản admin</exception>
        Task DeleteAccountAsync(int userId);
    }
} 
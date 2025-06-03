using SonitCustom.BLL.DTOs.Roles;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.BLL.Interface
{
    /// <summary>
    /// Service xử lý các thao tác liên quan đến role người dùng
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Lấy danh sách tất cả các role
        /// </summary>
        /// <returns>Danh sách các đối tượng <see cref="RoleDTO"/></returns>
        Task<List<RoleDTO>> GetAllRolesAsync();

        /// <summary>
        /// Lấy thông tin role theo ID
        /// </summary>
        /// <param name="roleId">ID của role cần lấy</param>
        /// <returns>Đối tượng <see cref="RoleDTO"/></returns>
        /// <exception cref="RoleNotFoundException">Ném ra khi không tìm thấy role</exception>
        Task<RoleDTO> GetRoleByIdAsync(int roleId);

        /// <summary>
        /// Tạo role mới
        /// </summary>
        /// <param name="createRole">Đối tượng <see cref="CreateRoleDTO"/> chứa thông tin role cần tạo</param>
        /// <exception cref="RoleNameAlreadyExistsException">Ném ra khi tên role đã tồn tại</exception>
        Task CreateRoleAsync(CreateRoleDTO createRole);

        /// <summary>
        /// Cập nhật thông tin role
        /// </summary>
        /// <param name="roleId">ID của role cần cập nhật</param>
        /// <param name="updateRole">Đối tượng <see cref="UpdateRoleDTO"/> chứa thông tin cập nhật cho role</param>
        /// <exception cref="RoleNotFoundException">Ném ra khi không tìm thấy role</exception>
        /// <exception cref="RoleNameAlreadyExistsException">Ném ra khi tên role mới đã tồn tại</exception>
        Task UpdateRoleAsync(int roleId, UpdateRoleDTO updateRole);

        /// <summary>
        /// Xóa role
        /// </summary>
        /// <param name="roleId">ID của role cần xóa</param>
        /// <exception cref="RoleNotFoundException">Ném ra khi không tìm thấy role</exception>
        /// <exception cref="RoleHasUsersException">Ném ra khi role vẫn còn được gán cho user</exception>
        Task DeleteRoleAsync(int roleId);
    }
}
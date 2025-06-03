using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Interface
{
    /// <summary>
    /// Interface định nghĩa các thao tác với <see cref="Role"/> trong cơ sở dữ liệu
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Kiểm tra role có tồn tại theo ID không
        /// </summary>
        /// <param name="roleId">ID của role cần kiểm tra</param>
        /// <returns>True nếu <see cref="Role"/> tồn tại, ngược lại False</returns>
        Task<bool> IsRoleExistsAsync(int roleId);
        
        /// <summary>
        /// Kiểm tra tên role đã tồn tại chưa
        /// </summary>
        /// <param name="roleName">Tên role cần kiểm tra</param>
        /// <returns>True nếu tên đã tồn tại, ngược lại False</returns>
        Task<bool> IsRoleNameExistsAsync(string roleName);
        
        /// <summary>
        /// Lấy tất cả role
        /// </summary>
        /// <returns>Danh sách các <see cref="Role"/></returns>
        Task<List<Role>> GetAllRolesAsync();
        
        /// <summary>
        /// Lấy role theo ID
        /// </summary>
        /// <param name="roleId">ID của role cần tìm</param>
        /// <returns><see cref="Role"/> tìm thấy hoặc null nếu không tồn tại</returns>
        Task<Role?> GetRoleByIdAsync(int roleId);
        
        /// <summary>
        /// Tạo mới một role
        /// </summary>
        /// <param name="role">Đối tượng role cần tạo</param>
        Task CreateRoleAsync(Role role);
        
        /// <summary>
        /// Cập nhật thông tin role
        /// </summary>
        /// <param name="role">Đối tượng role cần cập nhật</param>
        Task UpdateRoleAsync(Role role);
        
        /// <summary>
        /// Xóa một role
        /// </summary>
        /// <param name="role">Đối tượng role cần xóa</param>
        Task DeleteRoleAsync(Role role);
    }
}
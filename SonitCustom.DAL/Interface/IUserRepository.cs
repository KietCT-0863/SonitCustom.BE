using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Interface
{
    /// <summary>
    /// Interface định nghĩa các thao tác với <see cref="User"/> trong cơ sở dữ liệu
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Lấy tất cả user
        /// </summary>
        /// <returns>Danh sách các <see cref="User"/></returns>
        Task<List<User>> GetAllUserAsync();
        
        /// <summary>
        /// Lấy user theo ID
        /// </summary>
        /// <param name="userId">ID của user cần tìm</param>
        /// <returns><see cref="User"/> tìm thấy hoặc null nếu không tồn tại</returns>
        Task<User?> GetUserByIdAsync(int userId);
        
        /// <summary>
        /// Thêm mới một user
        /// </summary>
        /// <param name="newUser">Đối tượng user cần thêm mới</param>
        Task AddNewUserAsync(User newUser);
        
        /// <summary>
        /// Cập nhật thông tin user
        /// </summary>
        /// <param name="userToUpdate">Đối tượng user cần cập nhật</param>
        Task UpdateUserAsync(User userToUpdate);
        
        /// <summary>
        /// Lấy user theo tên đăng nhập và mật khẩu
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns><see cref="User"/> tìm thấy hoặc null nếu không tồn tại</returns>
        Task<User?> GetUserAsync(string username, string password);
        
        /// <summary>
        /// Lấy tên role của user theo ID
        /// </summary>
        /// <param name="userId">ID của user</param>
        /// <returns>Tên <see cref="Role"/> hoặc null nếu không tồn tại</returns>
        Task<string?> GetRoleByUserIdAsync(int userId);
        
        /// <summary>
        /// Kiểm tra tài khoản user đã tồn tại theo tên đăng nhập hoặc email
        /// </summary>
        /// <param name="username">Tên đăng nhập cần kiểm tra</param>
        /// <param name="email">Email cần kiểm tra</param>
        /// <returns>True nếu tên đăng nhập hoặc email đã tồn tại, ngược lại False</returns>
        Task<bool> CheckUserExistsAsync(string username, string email);
        
        /// <summary>
        /// Kiểm tra tên đăng nhập đã tồn tại chưa
        /// </summary>
        /// <param name="username">Tên đăng nhập cần kiểm tra</param>
        /// <param name="excludeUserId">ID user cần loại trừ khỏi việc kiểm tra (mặc định là 0)</param>
        /// <returns>True nếu tên đăng nhập đã tồn tại, ngược lại False</returns>
        Task<bool> IsUserNameExistsAsync(string username, int excludeUserId = 0);
        
        /// <summary>
        /// Kiểm tra email đã tồn tại chưa
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <param name="excludeUserId">ID user cần loại trừ khỏi việc kiểm tra (mặc định là 0)</param>
        /// <returns>True nếu email đã tồn tại, ngược lại False</returns>
        Task<bool> IsEmailExistsAsync(string email, int excludeUserId = 0);
        
        /// <summary>
        /// Xóa một user
        /// </summary>
        /// <param name="userToDelete">Đối tượng user cần xóa</param>
        Task DeleteUserAsync(User userToDelete);
        
        /// <summary>
        /// Đếm số lượng user có role cụ thể
        /// </summary>
        /// <param name="roleId">ID của role</param>
        /// <returns>Số lượng <see cref="User"/> có <see cref="Role"/> tương ứng</returns>
        Task<int> CountUsersByRoleIdAsync(int roleId);
    }
}
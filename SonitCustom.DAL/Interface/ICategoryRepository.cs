using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Interface
{
    /// <summary>
    /// Interface định nghĩa các thao tác với <see cref="Category"/> trong cơ sở dữ liệu
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Lấy tất cả Category
        /// </summary>
        /// <returns>Danh sách các <see cref="Category"/></returns>
        Task<List<Category>> GetAllCategoriesAsync();
        
        /// <summary>
        /// Tạo mới một Category
        /// </summary>
        /// <param name="category">Đối tượng <see cref="Category"/> cần tạo</param>
        Task CreateCategoryAsync(Category category);
        
        /// <summary>
        /// Lấy Category theo tên
        /// </summary>
        /// <param name="cateName">Tên Category cần tìm</param>
        /// <returns><see cref="Category"/> tìm thấy hoặc null nếu không tồn tại</returns>
        Task<Category?> GetCategoryByNameAsync(string cateName);
        
        /// <summary>
        /// Kiểm tra prefix đã tồn tại chưa
        /// </summary>
        /// <param name="prefix">Prefix cần kiểm tra</param>
        /// <returns>True nếu prefix đã tồn tại, ngược lại False</returns>
        Task<bool> CheckPrefixExistsAsync(string prefix);
        
        /// <summary>
        /// Cập nhật thông tin Category
        /// </summary>
        /// <param name="category">Đối tượng <see cref="Category"/> cần cập nhật</param>
        Task UpdateCategoryAsync(Category category);
        
        /// <summary>
        /// Lấy Category theo ID
        /// </summary>
        /// <param name="id">ID của Category cần tìm</param>
        /// <returns><see cref="Category"/> tìm thấy hoặc null nếu không tồn tại</returns>
        Task<Category?> GetCategoryByIdAsync(int? id);
        
        /// <summary>
        /// Kiểm tra tên Category đã tồn tại chưa
        /// </summary>
        /// <param name="cateName">Tên <see cref="Category"/> cần kiểm tra</param>
        /// <returns>True nếu tên đã tồn tại, ngược lại False</returns>
        Task<bool> IsCategoryExistAsync(string cateName);
        
        /// <summary>
        /// Xóa một Category
        /// </summary>
        /// <param name="category">Đối tượng <see cref="Category"/> cần xóa</param>
        Task DeleteCategoryAsync(Category category);
    }
}
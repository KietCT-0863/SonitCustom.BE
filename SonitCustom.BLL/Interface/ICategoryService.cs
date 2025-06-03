using SonitCustom.BLL.DTOs.Categories;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.BLL.Interface
{
    /// <summary>
    /// Service xử lý các thao tác liên quan đến category sản phẩm
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Lấy danh sách tất cả category sản phẩm
        /// </summary>
        /// <returns>Danh sách các đối tượng <see cref="CategoryDTO"/></returns>
        Task<List<CategoryDTO>> GetAllCategoriesAsync();

        /// <summary>
        /// Tạo category sản phẩm mới với tên được chỉ định
        /// </summary>
        /// <param name="categoryName">Tên category cần tạo</param>
        /// <exception cref="CategoryNameAlreadyExistsException">Ném ra khi tên category đã tồn tại</exception>
        Task CreateCategoryAsync(string categoryName);

        /// <summary>
        /// Cập nhật thông tin cho category sản phẩm
        /// </summary>
        /// <param name="cateId">ID của category cần cập nhật</param>
        /// <param name="categoryDTO">Dữ liệu cập nhật cho category của lớp <see cref="UpdateCategoryDTO"/></param>
        /// <exception cref="CategoryNotFoundException">Ném ra khi không tìm thấy category</exception>
        /// <exception cref="CategoryNameAlreadyExistsException">Ném ra khi tên category mới đã tồn tại</exception>
        /// <exception cref="CategoryPrefixAlreadyExistsException">Ném ra khi mã tiền tố mới đã tồn tại</exception>
        /// <remarks>Phương thức này sẽ tự động cập nhật mã sản phẩm cho tất cả sản phẩm thuộc category</remarks>
        Task UpdateCategoryAsync(int cateId, UpdateCategoryDTO categoryDTO);

        /// <summary>
        /// Xóa category sản phẩm
        /// </summary>
        /// <param name="cateId">ID của category cần xóa</param>
        /// <exception cref="CategoryNotFoundException">Ném ra khi không tìm thấy category</exception>
        /// <exception cref="CategoryHasProductsException">Ném ra khi category vẫn còn sản phẩm</exception>
        Task DeleteCategoryAsync(int cateId);
    }
}
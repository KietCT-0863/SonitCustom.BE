using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Interface
{
    /// <summary>
    /// Interface định nghĩa các thao tác với <see cref="Product"/> trong cơ sở dữ liệu
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Lấy tất cả product
        /// </summary>
        /// <returns>Danh sách các <see cref="Product"/></returns>
        Task<List<Product>> GetAllProductsAsync();
        
        /// <summary>
        /// Tạo mới một product
        /// </summary>
        /// <param name="product">Đối tượng product cần tạo</param>
        Task CreateProductAsync(Product product);
        
        /// <summary>
        /// Lấy danh sách product theo prefix của mã sản phẩm
        /// </summary>
        /// <param name="prefix">Prefix cần tìm kiếm</param>
        /// <returns>Danh sách <see cref="Product"/> có mã sản phẩm bắt đầu bằng prefix</returns>
        Task<List<Product>> GetProductsByPrefixIdAsync(string prefix);
        
        /// <summary>
        /// Cập nhật thông tin product
        /// </summary>
        /// <param name="product">Đối tượng product cần cập nhật</param>
        Task UpdateProductAsync(Product product);
        
        /// <summary>
        /// Lấy product theo mã sản phẩm
        /// </summary>
        /// <param name="proId">Mã sản phẩm cần tìm</param>
        /// <returns><see cref="Product"/> tìm thấy hoặc null nếu không tồn tại</returns>
        Task<Product?> GetProductByProIdAsync(string proId);
        
        /// <summary>
        /// Xóa một product
        /// </summary>
        /// <param name="product">Đối tượng <see cref="Product"/> cần xóa</param>
        Task DeleteProductAsync(Product product);
        
        /// <summary>
        /// Lấy tất cả product thuộc một category
        /// </summary>
        /// <param name="cateId">ID của category</param>
        /// <returns>Danh sách <see cref="Product"/> thuộc <see cref="Category"/></returns>
        Task<List<Product>> GetAllProductOfCategoryAsync(int cateId);
        
        /// <summary>
        /// Đếm số lượng product trong một category
        /// </summary>
        /// <param name="cateId">ID của category</param>
        /// <returns>Số lượng <see cref="Product"/> thuộc <see cref="Category"/></returns>
        Task<int> GetProductCountByCategoryIdAsync(int cateId);
    }
}
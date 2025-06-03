using Microsoft.AspNetCore.Http;
using SonitCustom.BLL.DTOs.Products;
using SonitCustom.BLL.Exceptions;
using SonitCustom.DAL.Entities;

namespace SonitCustom.BLL.Interface
{
    /// <summary>
    /// Service xử lý các thao tác liên quan đến sản phẩm
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Lấy danh sách tất cả sản phẩm
        /// </summary>
        /// <returns>Danh sách các đối tượng <see cref="ProductDTO"/></returns>
        Task<List<ProductDTO>> GetAllProductsAsync();

        /// <summary>
        /// Tạo sản phẩm mới kèm hình ảnh
        /// </summary>
        /// <param name="productData">Đối tượng <see cref="CreateProductDataDTO"/> chứa dữ liệu sản phẩm cần tạo</param>
        /// <param name="productImage">File hình ảnh sản phẩm</param>
        /// <exception cref="ArgumentNullException">Ném ra khi dữ liệu trống</exception>
        /// <exception cref="CategoryNotFoundException">Ném ra khi không tìm thấy danh mục</exception>
        Task CreateProductWithImageAsync(CreateProductDataDTO productData, IFormFile productImage);

        /// <summary>
        /// Cập nhật sản phẩm và hình ảnh liên quan
        /// </summary>
        /// <param name="prodId">Mã sản phẩm cần cập nhật</param>
        /// <param name="productData">Đối tượng <see cref="UpdateProductDTO"/> chứa dữ liệu cập nhật</param>
        /// <param name="imageData">Đối tượng <see cref="UpdateProductImageDTO"/> chứa dữ liệu hình ảnh mới (nếu có)</param>
        /// <exception cref="ArgumentNullException">Ném ra khi dữ liệu trống</exception>
        /// <exception cref="ProductNotFoundException">Ném ra khi không tìm thấy sản phẩm</exception>
        /// <exception cref="CategoryNotFoundException">Ném ra khi không tìm thấy danh mục</exception>
        Task UpdateProductWithImageAsync(string prodId, UpdateProductDTO productData, UpdateProductImageDTO? imageData);

        /// <summary>
        /// Xóa sản phẩm và hình ảnh liên quan
        /// </summary>
        /// <param name="prodId">Mã sản phẩm cần xóa</param>
        /// <exception cref="ProductNotFoundException">Ném ra khi không tìm thấy sản phẩm</exception>
        Task DeleteProductAsync(string prodId);

        /// <summary>
        /// Tạo lại mã sản phẩm sau khi danh mục được cập nhật
        /// </summary>
        /// <param name="oldProductId">Mã sản phẩm cũ</param>
        /// <param name="updatedCategory">Đối tượng <see cref="Category"/> chứa thông tin danh mục đã cập nhật</param>
        /// <exception cref="ProductNotFoundException">Ném ra khi không tìm thấy sản phẩm</exception>
        Task RegenerateProductIdAfterCategoryUpdate(string oldProductId, Category updatedCategory);
    }
}
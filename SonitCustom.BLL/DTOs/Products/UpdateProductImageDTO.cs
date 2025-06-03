using Microsoft.AspNetCore.Http;

namespace SonitCustom.BLL.DTOs.Products
{
    /// <summary>
    /// DTO dùng để cập nhật hình ảnh của sản phẩm
    /// </summary>
    public class UpdateProductImageDTO
    {
        /// <summary>
        /// File hình ảnh mới của sản phẩm (null nếu không cập nhật hình ảnh)
        /// </summary>
        public IFormFile? ProductImage { get; set; }
    }
}
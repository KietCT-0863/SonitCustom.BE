using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SonitCustom.BLL.DTOs.Products
{
    /// <summary>
    /// DTO dùng để nhận file hình ảnh khi tạo mới sản phẩm
    /// </summary>
    public class CreateProductImageDTO
    {
        /// <summary>
        /// File hình ảnh của sản phẩm
        /// </summary>
        /// <remarks>Bắt buộc phải có</remarks>
        [Required(ErrorMessage = "Ảnh sản phẩm không được để trống")]
        public IFormFile ProductImage { get; set; }
    }
} 
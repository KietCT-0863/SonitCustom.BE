using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs.Categories
{
    /// <summary>
    /// DTO dùng để tạo mới category sản phẩm
    /// </summary>
    public class CreateCategoryDTO
    {
        /// <summary>
        /// Tên category mới
        /// </summary>
        /// <remarks>Bắt buộc phải có, không được trùng với tên category đã tồn tại</remarks>
        [Required(ErrorMessage = "Tên category không được để trống")]
        public string CategoryName { get; set; }
    }
} 
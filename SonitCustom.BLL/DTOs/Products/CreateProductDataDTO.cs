using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs.Products
{
    /// <summary>
    /// DTO dùng để nhận dữ liệu từ form tạo mới sản phẩm (không bao gồm hình ảnh)
    /// </summary>
    public class CreateProductDataDTO
    {
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        /// <remarks>Bắt buộc phải có, độ dài từ 3-100 ký tự</remarks>
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên sản phẩm phải có từ 3 đến 100 ký tự")]
        public string ProName { get; set; }

        /// <summary>
        /// Mô tả sản phẩm
        /// </summary>
        /// <remarks>Bắt buộc phải có</remarks>
        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }

        /// <summary>
        /// Giá sản phẩm
        /// </summary>
        /// <remarks>Giá phải lớn hơn hoặc bằng 0, giá = 0 sẽ được đánh dấu là sản phẩm tùy chỉnh</remarks>
        [Range(0, double.MaxValue, ErrorMessage = "Giá không được âm")]
        public double Price { get; set; }

        /// <summary>
        /// ID category của sản phẩm
        /// </summary>
        /// <remarks>Bắt buộc phải có</remarks>
        [Required(ErrorMessage = "Category không được để trống")]
        public int Category { get; set; }

        /// <summary>
        /// Đánh dấu những sản phẩm có giá liên hệ
        /// </summary>
        /// <remarks>Bắt buộc phải có</remarks>
        [Required(ErrorMessage = "IsCustom không được để trống")]
        public bool IsCustom { get; set; }
    }
} 
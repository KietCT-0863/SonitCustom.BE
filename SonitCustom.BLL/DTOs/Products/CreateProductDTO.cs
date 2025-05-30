using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs.Products
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên sản phẩm phải có từ 3 đến 100 ký tự")]
        public string ProName { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }

        [Required(ErrorMessage = "URL ảnh không được để trống")]
        public string ImgUrl { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá không được âm")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Danh mục không được để trống")]
        public int Category { get; set; }
    }
} 
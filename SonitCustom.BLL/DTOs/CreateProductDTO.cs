using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs
{
    public class CreateProductDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string ProName { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }

        [Required(ErrorMessage = "URL hình ảnh không được để trống")]
        public string ImgUrl { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Giá chỉ được chứa số")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Category không được để trống")]
        public string Category { get; set; }

        [Required(ErrorMessage = "IsCustom không được để trống")]
        public bool IsCustom { get; set; }
    }
}

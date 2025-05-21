using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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
        public string Price { get; set; }

        [Required(ErrorMessage = "Danh mục không được để trống")]
        public string Category { get; set; }
    }
}

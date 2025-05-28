using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs
{
    public class UpdateProductDTO
    {
        public string ProName { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Giá chỉ được chứa số")]
        public double Price { get; set; }

        public string Category { get; set; }
    }
}

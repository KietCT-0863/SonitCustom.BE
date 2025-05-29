namespace SonitCustom.BLL.DTOs.Products
{
    public class UpdateProductDTO
    {
        public string ProName { get; set; }

        public string Description { get; set; }

        public string ImgUrl { get; set; }

        public double? Price { get; set; }

        public string Category { get; set; }

        public bool? IsCustom { get; set; }
    }
} 
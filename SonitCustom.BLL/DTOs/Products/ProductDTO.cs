namespace SonitCustom.BLL.DTOs.Products
{
    public class ProductDTO
    {
        public string ProdId { get; set; }

        public string ProName { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string ImgUrl { get; set; }

        public int Category { get; set; }

        public bool IsCustom { get; set; }
    }
} 
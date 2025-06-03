namespace SonitCustom.BLL.DTOs.Products
{
    /// <summary>
    /// DTO chứa thông tin sản phẩm để hiển thị
    /// </summary>
    public class ProductDTO
    {
        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        public string ProdId { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProName { get; set; }

        /// <summary>
        /// Mô tả sản phẩm
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Giá sản phẩm
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Đường dẫn hình ảnh sản phẩm
        /// </summary>
        public string ImgUrl { get; set; }

        /// <summary>
        /// Tên category của sản phẩm
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Đánh dấu những sản phẩm có giá liên hệ
        /// </summary>
        public bool IsCustom { get; set; }
    }
}
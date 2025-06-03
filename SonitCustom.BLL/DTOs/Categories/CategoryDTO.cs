namespace SonitCustom.BLL.DTOs.Categories
{
    /// <summary>
    /// DTO chứa thông tin category sản phẩm
    /// </summary>
    public class CategoryDTO
    {
        /// <summary>
        /// ID của category
        /// </summary>
        public int CateId { get; set; }

        /// <summary>
        /// Tên category
        /// </summary>
        public string CateName { get; set; }

        /// <summary>
        /// Mã tiền tố dùng để tạo mã sản phẩm
        /// </summary>
        public string Prefix { get; set; }
    }
} 
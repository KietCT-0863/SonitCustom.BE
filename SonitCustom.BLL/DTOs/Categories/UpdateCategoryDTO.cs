namespace SonitCustom.BLL.DTOs.Categories
{
    /// <summary>
    /// DTO dùng để cập nhật thông tin category sản phẩm
    /// </summary>
    public class UpdateCategoryDTO
    {
        /// <summary>
        /// Tên category mới (null nếu không cập nhật tên)
        /// </summary>
        public string? CateName { get; set; }

        /// <summary>
        /// Mã tiền tố mới (null nếu không cập nhật mã tiền tố)
        /// </summary>
        /// <remarks>Khi cập nhật mã tiền tố, tất cả mã sản phẩm thuộc category này sẽ được tạo lại</remarks>
        public string? Prefix { get; set; }
    }
} 
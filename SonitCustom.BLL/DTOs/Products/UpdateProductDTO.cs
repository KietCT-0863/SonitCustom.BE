namespace SonitCustom.BLL.DTOs.Products
{
    /// <summary>
    /// DTO dùng để cập nhật thông tin sản phẩm
    /// </summary>
    public class UpdateProductDTO
    {
        /// <summary>
        /// Tên sản phẩm (null nếu không cập nhật)
        /// </summary>
        public string? ProName { get; set; }

        /// <summary>
        /// Mô tả sản phẩm (null nếu không cập nhật)
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Đường dẫn hình ảnh sản phẩm
        /// </summary>
        /// <remarks>ImgUrl sẽ được thiết lập bởi service, không phải từ form input</remarks>
        internal string? ImgUrl { get; set; }

        /// <summary>
        /// Giá sản phẩm (null nếu không cập nhật)
        /// </summary>
        public double? Price { get; set; }

        /// <summary>
        /// ID category của sản phẩm (null nếu không cập nhật)
        /// </summary>
        public int? Category { get; set; }

        /// <summary>
        /// Đánh dấu những sản phẩm có giá liên hệ (null nếu không cập nhật)
        /// </summary>
        public bool? IsCustom { get; set; }
    }
} 
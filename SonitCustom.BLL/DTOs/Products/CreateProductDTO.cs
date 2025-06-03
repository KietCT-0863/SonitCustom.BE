using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs.Products
{
    /// <summary>
    /// DTO dùng để tạo mới sản phẩm
    /// </summary>
    public class CreateProductDTO
    {
        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        public string ProName { get; set; }
        
        /// <summary>
        /// Mô tả sản phẩm
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Đường dẫn hình ảnh sản phẩm
        /// </summary>
        public string ImgUrl { get; set; }
        
        /// <summary>
        /// Giá sản phẩm (giá = 0 sẽ được đánh dấu là sản phẩm tùy chỉnh)
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// ID category của sản phẩm
        /// </summary>
        public int Category { get; set; }

        /// <summary>
        /// Đánh dấu những sản phẩm có giá liên hệ
        /// </summary>
        public bool IsCustom { get; set; }
    }
} 
namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi không tìm thấy sản phẩm
    /// </summary>
    public class ProductNotFoundException : Exception
    {
        /// <summary>
        /// ID của sản phẩm không tìm thấy
        /// </summary>
        public string ProductId { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="ProductNotFoundException"/> với ID sản phẩm được chỉ định
        /// </summary>
        /// <param name="id">ID của sản phẩm không tìm thấy</param>
        public ProductNotFoundException(string id)
            : base($"Product có ID '{id}' không tồn tại")
        {
            ProductId = id;
        }
    }
} 
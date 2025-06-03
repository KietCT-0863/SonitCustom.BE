namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi không tìm thấy category
    /// </summary>
    public class CategoryNotFoundException : Exception
    {
        /// <summary>
        /// Tên của category không tìm thấy
        /// </summary>
        public string? CategoryName { get; }

        /// <summary>
        /// ID của category không tìm thấy
        /// </summary>
        public int? CategoryId { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryNotFoundException"/> với thông báo mặc định
        /// </summary>
        public CategoryNotFoundException() : base("Không tìm thấy Category") { }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryNotFoundException"/> với tên category được chỉ định
        /// </summary>
        /// <param name="cateName">Tên của category không tìm thấy</param>
        public CategoryNotFoundException(string cateName) 
            : base($"Không tìm thấy Category với tên: {cateName}") 
        { 
            CategoryName = cateName;
        }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryNotFoundException"/> với thông báo và ngoại lệ bên trong
        /// </summary>
        /// <param name="message">Thông báo lỗi</param>
        /// <param name="innerException">Ngoại lệ bên trong</param>
        public CategoryNotFoundException(string message, Exception innerException) 
            : base(message, innerException) { }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryNotFoundException"/> với ID category được chỉ định
        /// </summary>
        /// <param name="categoryId">ID của category không tìm thấy</param>
        public CategoryNotFoundException(int? categoryId)
            : base($"Không tìm thấy Category với ID: {categoryId}") 
        { 
            CategoryId = categoryId;
        }
    }
} 
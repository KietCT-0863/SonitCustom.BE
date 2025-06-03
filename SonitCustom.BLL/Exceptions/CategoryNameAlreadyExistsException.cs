namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi cố gắng tạo hoặc đổi tên category với tên đã tồn tại
    /// </summary>
    public class CategoryNameAlreadyExistsException : Exception
    {
        /// <summary>
        /// Tên category đã tồn tại
        /// </summary>
        public string? CategoryName { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryNameAlreadyExistsException"/> với thông báo mặc định
        /// </summary>
        public CategoryNameAlreadyExistsException() : base("Tên Category đã tồn tại") { }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryNameAlreadyExistsException"/> với thông báo và ngoại lệ bên trong được chỉ định
        /// </summary>
        /// <param name="message">Thông báo lỗi</param>
        /// <param name="innerException">Ngoại lệ bên trong</param>
        public CategoryNameAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryNameAlreadyExistsException"/> với tên category được chỉ định
        /// </summary>
        /// <param name="categoryName">Tên category đã tồn tại</param>
        public CategoryNameAlreadyExistsException(string categoryName)
            : base($"Category có tên '{categoryName}' đã tồn tại")
        {
            CategoryName = categoryName;
        }
    }
} 
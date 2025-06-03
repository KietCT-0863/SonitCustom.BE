namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi cố gắng tạo hoặc cập nhật category với mã tiền tố đã tồn tại
    /// </summary>
    public class CategoryPrefixAlreadyExistsException : Exception
    {
        /// <summary>
        /// Mã tiền tố đã tồn tại
        /// </summary>
        public string? Prefix { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryPrefixAlreadyExistsException"/> với thông báo mặc định
        /// </summary>
        public CategoryPrefixAlreadyExistsException() : base("Prefix đã tồn tại") { }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryPrefixAlreadyExistsException"/> với mã tiền tố được chỉ định
        /// </summary>
        /// <param name="prefix">Mã tiền tố đã tồn tại</param>
        public CategoryPrefixAlreadyExistsException(string prefix)
            : base($"Mã tiền tố '{prefix}' đã tồn tại") 
        {
            Prefix = prefix;
        }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryPrefixAlreadyExistsException"/> với thông báo và ngoại lệ bên trong được chỉ định
        /// </summary>
        /// <param name="message">Thông báo lỗi</param>
        /// <param name="innerException">Ngoại lệ bên trong</param>
        public CategoryPrefixAlreadyExistsException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
} 
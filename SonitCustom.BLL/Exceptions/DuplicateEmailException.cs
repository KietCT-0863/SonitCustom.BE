namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi cố gắng cập nhật thông tin người dùng với email đã tồn tại
    /// </summary>
    public class DuplicateEmailException : Exception
    {
        /// <summary>
        /// Email trùng lặp
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="DuplicateEmailException"/> với email được chỉ định
        /// </summary>
        /// <param name="email">Email đã tồn tại</param>
        public DuplicateEmailException(string email)
            : base($"Email '{email}' đã tồn tại trong hệ thống")
        {
            Email = email;
        }
    }
} 
namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi cố gắng cập nhật thông tin người dùng với tên đăng nhập đã tồn tại
    /// </summary>
    public class DuplicateUserNameException : Exception
    {
        /// <summary>
        /// Tên đăng nhập trùng lặp
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="DuplicateUserNameException"/> với tên đăng nhập được chỉ định
        /// </summary>
        /// <param name="username">Tên đăng nhập đã tồn tại</param>
        public DuplicateUserNameException(string username)
            : base($"Tên đăng nhập '{username}' đã tồn tại trong hệ thống")
        {
            Username = username;
        }
    }
} 
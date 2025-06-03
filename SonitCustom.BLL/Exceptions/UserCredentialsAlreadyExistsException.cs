namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi cố gắng đăng ký tài khoản với tên đăng nhập hoặc email đã tồn tại
    /// </summary>
    public class UserCredentialsAlreadyExistsException : Exception
    {
        /// <summary>
        /// Tên đăng nhập đã tồn tại
        /// </summary>
        public string UserName { get; }
        
        /// <summary>
        /// Email đã tồn tại
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="UserCredentialsAlreadyExistsException"/> với tên đăng nhập và email được chỉ định
        /// </summary>
        /// <param name="userName">Tên đăng nhập đã tồn tại</param>
        /// <param name="email">Email đã tồn tại</param>
        public UserCredentialsAlreadyExistsException(string userName, string email)
            : base($"Tên đăng nhập '{userName}' hoặc email '{email}' đã tồn tại trong hệ thống")
        {
            UserName = userName;
            Email = email;
        }
    }
}
namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi thông tin đăng nhập không chính xác
    /// </summary>
    public class InvalidCredentialsException : Exception
    {
        /// <summary>
        /// Tên đăng nhập được sử dụng trong nỗ lực đăng nhập không thành công
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="InvalidCredentialsException"/> với tên đăng nhập được chỉ định
        /// </summary>
        /// <param name="username">Tên đăng nhập không hợp lệ</param>
        public InvalidCredentialsException(string username)
            : base($"Đăng nhập thất bại: Tên đăng nhập hoặc mật khẩu không đúng") 
        { 
            Username = username;
        }
    }
}
namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi refresh token không hợp lệ
    /// </summary>
    public class InvalidRefreshTokenException : Exception
    {
        /// <summary>
        /// Thông báo liên quan đến token không hợp lệ
        /// </summary>
        public string TokenMessage { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="InvalidRefreshTokenException"/> với thông báo được chỉ định
        /// </summary>
        /// <param name="message">Thông báo lỗi</param>
        public InvalidRefreshTokenException(string message) : base(message) 
        {
            TokenMessage = message;
        }
    }
}

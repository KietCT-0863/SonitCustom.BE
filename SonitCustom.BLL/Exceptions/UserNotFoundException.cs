namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi không tìm thấy người dùng
    /// </summary>
    public class UserNotFoundException : Exception
    {
        /// <summary>
        /// ID của người dùng không tìm thấy
        /// </summary>
        public int UserId { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="UserNotFoundException"/> với ID người dùng được chỉ định
        /// </summary>
        /// <param name="userId">ID của người dùng không tìm thấy</param>
        public UserNotFoundException(int userId)
            : base($"Không tìm thấy người dùng với ID: {userId}")
        {
            UserId = userId;
        }
    }
} 
namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi cố gắng xóa một tài khoản có quyền Admin
    /// </summary>
    public class AdminDeleteException : Exception
    {
        /// <summary>
        /// Khởi tạo một instance mới của <see cref="AdminDeleteException"/> với ID người dùng được chỉ định
        /// </summary>
        /// <param name="userId">ID của tài khoản Admin không thể xóa</param>
        public AdminDeleteException(int userId)
            : base("Không được phép xoá tài khoản Admin")
        {
        }
    }
} 
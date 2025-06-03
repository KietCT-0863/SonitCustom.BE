namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi không tìm thấy role
    /// </summary>
    public class RoleNotFoundException : Exception
    {
        /// <summary>
        /// ID của role không tìm thấy
        /// </summary>
        public int RoleId { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="RoleNotFoundException"/> với ID role được chỉ định
        /// </summary>
        /// <param name="roleId">ID của role không tìm thấy</param>
        public RoleNotFoundException(int roleId)
            : base($"Không tìm thấy role với ID: {roleId}")
        {
            RoleId = roleId;
        }
    }
} 
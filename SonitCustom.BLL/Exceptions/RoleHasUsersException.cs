namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi cố gắng xóa một role vẫn đang được gán cho người dùng
    /// </summary>
    public class RoleHasUsersException : Exception
    {
        /// <summary>
        /// ID của role không thể xóa
        /// </summary>
        public int RoleId { get; }

        /// <summary>
        /// Số lượng người dùng đang sử dụng role
        /// </summary>
        public int UserCount { get; }

        /// <summary>
        /// Khởi tạo một instance mới của <see cref="RoleHasUsersException"/> với ID role và số lượng người dùng được chỉ định
        /// </summary>
        /// <param name="roleId">ID của role không thể xóa</param>
        /// <param name="userCount">Số lượng người dùng đang sử dụng role</param>
        public RoleHasUsersException(int roleId, int userCount)
            : base($"Không thể xóa role ID {roleId} vì vẫn còn {userCount} người dùng đang sử dụng role này.")
        {
            RoleId = roleId;
            UserCount = userCount;
        }
    }
} 
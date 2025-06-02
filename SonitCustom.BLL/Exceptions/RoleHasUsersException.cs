namespace SonitCustom.BLL.Exceptions
{
    public class RoleHasUsersException : Exception
    {
        public int RoleId { get; }
        public int UserCount { get; }

        public RoleHasUsersException(int roleId, int userCount)
            : base($"Không thể xóa role ID {roleId} vì vẫn còn {userCount} người dùng đang sử dụng role này.")
        {
            RoleId = roleId;
            UserCount = userCount;
        }
    }
} 
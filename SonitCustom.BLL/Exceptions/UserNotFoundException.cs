namespace SonitCustom.BLL.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public int UserId { get; }

        public UserNotFoundException(int userId)
            : base($"Không tìm thấy người dùng với ID: {userId}")
        {
            UserId = userId;
        }
    }
} 
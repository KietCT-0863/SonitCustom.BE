namespace SonitCustom.BLL.Exceptions
{
    public class DuplicateUserNameException : Exception
    {
        public string Username { get; }

        public DuplicateUserNameException(string username)
            : base($"Tên đăng nhập '{username}' đã tồn tại trong hệ thống")
        {
            Username = username;
        }
    }
} 
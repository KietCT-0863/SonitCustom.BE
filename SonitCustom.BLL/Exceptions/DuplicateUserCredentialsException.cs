namespace SonitCustom.BLL.Exceptions
{
    public class DuplicateUserCredentialsException : Exception
    {
        public DuplicateUserCredentialsException(string userName, string email)
            : base($"Tên đăng nhập '{userName}' hoặc email '{email}' đã tồn tại trong hệ thống")
        {
        }
    }
}
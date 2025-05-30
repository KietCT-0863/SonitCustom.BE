namespace SonitCustom.BLL.Exceptions
{
    public class UserCredentialsAlreadyExistsException : Exception
    {
        public UserCredentialsAlreadyExistsException(string userName, string email)
            : base($"Tên đăng nhập '{userName}' hoặc email '{email}' đã tồn tại trong hệ thống")
        {
        }
    }
}
namespace SonitCustom.BLL.Exceptions
{
    public class UserCredentialsAlreadyExistsException : Exception
    {
        public string UserName { get; }
        public string Email { get; }

        public UserCredentialsAlreadyExistsException(string userName, string email)
            : base($"Tên đăng nhập '{userName}' hoặc email '{email}' đã tồn tại trong hệ thống")
        {
            UserName = userName;
            Email = email;
        }
    }
}
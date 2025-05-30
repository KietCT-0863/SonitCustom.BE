namespace SonitCustom.BLL.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string username)
            : base($"Đăng nhập thất bại: Tên đăng nhập hoặc mật khẩu không đúng") { }
    }
}
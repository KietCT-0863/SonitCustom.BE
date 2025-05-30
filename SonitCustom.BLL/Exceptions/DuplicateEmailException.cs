namespace SonitCustom.BLL.Exceptions
{
    public class DuplicateEmailException : Exception
    {
        public string Email { get; }

        public DuplicateEmailException(string email)
            : base($"Email '{email}' đã tồn tại trong hệ thống")
        {
            Email = email;
        }
    }
} 
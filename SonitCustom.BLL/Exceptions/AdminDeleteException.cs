namespace SonitCustom.BLL.Exceptions
{
    public class AdminDeleteException : Exception
    {
        public AdminDeleteException(int userId)
            : base("Không được phép xoá tài khoản Admin")
        {
        }
    }
} 
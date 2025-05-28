namespace SonitCustom.BLL.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string id)
            : base($"Product có ID '{id}' không tồn tại")
        {
        }
    }
} 
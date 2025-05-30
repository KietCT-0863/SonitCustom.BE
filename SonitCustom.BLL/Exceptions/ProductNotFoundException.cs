namespace SonitCustom.BLL.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public string ProductId { get; }

        public ProductNotFoundException(string id)
            : base($"Product có ID '{id}' không tồn tại")
        {
            ProductId = id;
        }
    }
} 
namespace SonitCustom.BLL.Exceptions
{
    public class CategoryPrefixAlreadyExistsException : Exception
    {
        public string? Prefix { get; }

        public CategoryPrefixAlreadyExistsException() : base("Prefix đã tồn tại") { }

        public CategoryPrefixAlreadyExistsException(string prefix)
            : base($"Mã tiền tố '{prefix}' đã tồn tại") 
        {
            Prefix = prefix;
        }

        public CategoryPrefixAlreadyExistsException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
} 
namespace SonitCustom.BLL.Exceptions
{
    public class CategoryNameAlreadyExistsException : Exception
    {
        public CategoryNameAlreadyExistsException() : base("Tên Category đã tồn tại") { }

        public CategoryNameAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }

        public CategoryNameAlreadyExistsException(string categoryName)
            : base($"Category có tên '{categoryName}' đã tồn tại") { }
    }
} 
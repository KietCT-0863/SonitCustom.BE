namespace SonitCustom.BLL.Exceptions
{
    public class CategoryNameExistException : Exception
    {
        public CategoryNameExistException() : base("Tên Category đã tồn tại") { }

        public CategoryNameExistException(string message, Exception innerException) : base(message, innerException) { }

        public CategoryNameExistException(string categoryName)
            : base($"Category có tên '{categoryName}' đã tồn tại") { }
    }
} 
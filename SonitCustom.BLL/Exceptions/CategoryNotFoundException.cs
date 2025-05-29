namespace SonitCustom.BLL.Exceptions
{
    public class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException() : base("Không tìm thấy Category") { }

        public CategoryNotFoundException(string cateName) 
            : base($"Không tìm thấy Category với tên: {cateName}") { }

        public CategoryNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public CategoryNotFoundException(int categoryId)
            : base($"Không tìm thấy Cateogry với ID: {categoryId}") { }
    }
} 
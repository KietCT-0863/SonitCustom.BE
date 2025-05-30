namespace SonitCustom.BLL.Exceptions
{
    public class CategoryNotFoundException : Exception
    {
        public string? CategoryName { get; }
        public int? CategoryId { get; }

        public CategoryNotFoundException() : base("Không tìm thấy Category") { }

        public CategoryNotFoundException(string cateName) 
            : base($"Không tìm thấy Category với tên: {cateName}") 
        { 
            CategoryName = cateName;
        }

        public CategoryNotFoundException(string message, Exception innerException) 
            : base(message, innerException) { }

        public CategoryNotFoundException(int? categoryId)
            : base($"Không tìm thấy Category với ID: {categoryId}") 
        { 
            CategoryId = categoryId;
        }
    }
} 
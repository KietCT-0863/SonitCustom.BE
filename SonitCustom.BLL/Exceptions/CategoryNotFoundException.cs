using System;

namespace SonitCustom.BLL.Exceptions
{
    public class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException() : base("Không tìm thấy Category") { }

        public CategoryNotFoundException(string message) : base(message) { }

        public CategoryNotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public CategoryNotFoundException(int categoryId)
            : base($"Không tìm thấy danh mục với ID: {categoryId}") { }
    }
} 
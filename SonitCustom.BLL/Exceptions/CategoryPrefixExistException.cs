using System;

namespace SonitCustom.BLL.Exceptions
{
    public class CategoryPrefixExistException : Exception
    {
        public CategoryPrefixExistException() : base("Prefix đã tồn tại") { }

        public CategoryPrefixExistException(string prefix)
            : base($"Mã tiền tố '{prefix}' đã tồn tại") { }

        public CategoryPrefixExistException(string message, Exception innerException) : base(message, innerException) { }
    }
} 
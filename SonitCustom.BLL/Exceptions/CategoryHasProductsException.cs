namespace SonitCustom.BLL.Exceptions
{
    /// <summary>
    /// Ngoại lệ được ném ra khi cố gắng xóa một category vẫn chứa sản phẩm
    /// </summary>
    public class CategoryHasProductsException : Exception
    {
        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CategoryHasProductsException"/> với ID category và số lượng sản phẩm được chỉ định
        /// </summary>
        /// <param name="categoryId">ID của category không thể xóa</param>
        /// <param name="productCount">Số lượng sản phẩm trong category</param>
        public CategoryHasProductsException(int categoryId, int productCount) 
            : base($"Không thể xóa category với ID {categoryId} vì có {productCount} sản phẩm thuộc category này.")
        {
        }
    }
} 
namespace SonitCustom.BLL.Exceptions
{
    public class CategoryHasProductsException : Exception
    {
        public CategoryHasProductsException(int categoryId, int productCount) 
            : base($"Không thể xóa danh mục với ID {categoryId} vì có {productCount} sản phẩm thuộc danh mục này.")
        {
        }
    }
} 
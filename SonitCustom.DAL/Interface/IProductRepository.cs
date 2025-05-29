using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task CreateProductAsync(Product product);
        Task<List<Product>> GetProductsByPrefixIdAsync(string prefix);
        Task UpdateProductAsync(Product product);
        Task<Product?> GetProductByProIdAsync(string proId);
        Task DeleteProductAsync(Product product);

        //Task<List<Product>> GetAllProductOfCategoryAsync(int cateId);
    }
}
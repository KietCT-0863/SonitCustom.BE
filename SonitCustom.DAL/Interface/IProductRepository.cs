using SonitCustom.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonitCustom.DAL.Interface
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Product product);
        Task<int> GetNumberOfProductByCategoryAsync(string category);
    }
} 
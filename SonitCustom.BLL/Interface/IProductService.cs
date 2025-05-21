using SonitCustom.BLL.DTOs;
using SonitCustom.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonitCustom.BLL.Interface
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(string id);
        Task<bool> CreateProductAsync(CreateProductDTO product);
        Task<Product> UpdateProductAsync(string id, UpdateProductDTO product);
        Task<bool> DeleteProductAsync(string id);
    }
} 
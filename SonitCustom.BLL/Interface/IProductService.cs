
using SonitCustom.BLL.DTOs;

namespace SonitCustom.BLL.Interface
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task CreateProductAsync(CreateProductDTO product);
        Task UpdateProductAsync(string proId, UpdateProductDTO product);
        Task DeleteProductAsync(string prodId);

        //Task<ProductDTO> GetProductByIdAsync(string id);

    }
}
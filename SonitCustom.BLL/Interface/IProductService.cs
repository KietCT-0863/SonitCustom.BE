using SonitCustom.BLL.DTOs.Products;
using SonitCustom.DAL.Entities;

namespace SonitCustom.BLL.Interface
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllProductsAsync();
        Task CreateProductAsync(CreateProductDTO product);
        Task UpdateProductAsync(string proId, UpdateProductDTO product);
        Task DeleteProductAsync(string prodId);
        Task RegenerateProductIdAfterCategoryUpdate(string oldProductId, Category updatedCategory);

        //Task<ProductDTO> GetProductByIdAsync(string id);

    }
}
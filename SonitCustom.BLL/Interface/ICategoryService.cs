using SonitCustom.BLL.DTOs;

namespace SonitCustom.BLL.Interface
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task CreateCategoryAsync(string categoryName);
        Task UpdateCategoryAsync(int cateId, UpdateCategoryDTO categoryDTO);
        Task DeleteCategoryAsync(int cateId);
    }
}
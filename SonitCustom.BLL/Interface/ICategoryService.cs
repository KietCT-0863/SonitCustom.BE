using SonitCustom.DAL.Entities;
using SonitCustom.BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

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
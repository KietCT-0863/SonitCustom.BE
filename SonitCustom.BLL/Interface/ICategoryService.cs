using SonitCustom.DAL.Entities;
using SonitCustom.BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonitCustom.BLL.Interface
{
    public interface ICategoryService
    {
        Task<bool> CreateCategoryAsync(string categoryName);
        Task<List<CategoryDTO>> GetAllCategoriesAsync();
        Task<bool> UpdateCategoryAsync();
    }
} 
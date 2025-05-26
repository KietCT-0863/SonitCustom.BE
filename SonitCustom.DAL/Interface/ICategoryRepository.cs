using SonitCustom.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonitCustom.DAL.Interface
{
    public interface ICategoryRepository
    {
        Task<int> GetCategoryIdByNameAsync(string cateName);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category> CreateCategoryAsync(Category category);
        Task<string> GetPrefixFromCategoryName(string categoryName);
        Task<bool> CheckPrefixExistsAsync(string prefix);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId);

    }
}
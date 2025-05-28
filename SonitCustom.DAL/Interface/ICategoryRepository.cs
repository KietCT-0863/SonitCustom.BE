using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Interface
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task CreateCategoryAsync(Category category);
        Task<Category?> GetCategoryByNameAsync(string cateName);
        Task<bool> CheckPrefixExistsAsync(string prefix);
        Task UpdateCategoryAsync(Category category);
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<bool> IsCategoryExistAsync(string cateName);
        Task DeleteCategoryAsync(Category category);
        //Task<int> GetCategoryIdByNameAsync(string cateName);

        //Task<string> GetPrefixFromCategoryName(string categoryName);

    }
}
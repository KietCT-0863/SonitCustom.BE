using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SonitCustom.BLL.DTOs;
using System.Collections.Generic;

namespace SonitCustom.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> CreateCategoryAsync(string categoryName)
        {
            string newPrefix = await GeneratePrefixFromCategory(categoryName);

            Category category = new()
            {
                CateName = categoryName,
                Prefix = newPrefix
            };
            await _categoryRepository.CreateCategoryAsync(category);
            return true;
        }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories.Select(c => new CategoryDTO
            {
                CateId = c.CateId,
                CateName = c.CateName,
                Prefix = c.Prefix
            }).ToList();
        }

        private async Task<string> GeneratePrefixFromCategory(string category)
        {
            // Tách category thành các từ
            var words = category.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string newPrefix;

            if (words.Length == 1)
            {
                // Nếu chỉ có 1 từ, lấy 3 chữ đầu tiên
                newPrefix = words[0].Length >= 3 ? words[0].Substring(0, 3) : words[0].PadRight(3, 'x');
            }
            else
            {
                // Nếu có nhiều từ, lấy chữ đầu của mỗi từ
                newPrefix = string.Join("", words.Select(w => w[0]));
            }

            // Kiểm tra prefix đã tồn tại chưa
            bool prefixExists = await _categoryRepository.CheckPrefixExistsAsync(newPrefix);
            
            // Nếu prefix đã tồn tại, trả về SONIT
            return prefixExists ? "sonit" : newPrefix;
        }
    }
}
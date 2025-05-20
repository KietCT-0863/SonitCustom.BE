using Microsoft.EntityFrameworkCore;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SonitCustom.DAL.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SonitCustomDBContext _context;

        public CategoryRepository(SonitCustomDBContext context)
        {
            _context = context;
        }

        public async Task<int> GetCategoryIdByNameAsync(string cateName)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CateName.ToLower() == cateName.ToLower());
            return category?.CateId ?? throw new ArgumentException($"Category with name '{cateName}' not found");
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<string> GetPrefixFromCategoryName(string categoryName)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CateName.ToLower() == categoryName.ToLower());
            return category?.Prefix ?? throw new ArgumentException($"Category with name '{categoryName}' not found");
        }

        public async Task<bool> CheckPrefixExistsAsync(string prefix)
        {
            return await _context.Categories
                .AnyAsync(c => c.Prefix.ToLower() == prefix.ToLower());
        }
    }
} 
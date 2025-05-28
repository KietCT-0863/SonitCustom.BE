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
        //private readonly SonitCustomDBContext _context;

        //public CategoryRepository(SonitCustomDBContext context)
        //{
        //    _context = context;
        //}

        //public async Task<bool> IsCategoryExistAsync(string cateName)
        //{
        //    return await _context.Categories
        //        .AnyAsync(c => c.CateName.ToLower() == cateName.ToLower());
        //}

        //public async Task<int> GetCategoryIdByNameAsync(string cateName)
        //{
        //    Category? category = await _context.Categories
        //        .FirstOrDefaultAsync(c => c.CateName.ToLower() == cateName.ToLower());

        //    return category?.CateId ?? throw new ArgumentException($"Category with name '{cateName}' not found");
        //}

        //public async Task<Category?> GetCategoryByIdAsync(int id)
        //{
        //    return await _context.Categories.FirstOrDefaultAsync(c => c.CateId == id);
        //}

        //public async Task<List<Category>> GetAllCategoriesAsync()
        //{
        //    return await _context.Categories.ToListAsync();
        //}

        //public async Task<Category> CreateCategoryAsync(Category category)
        //{
        //    await _context.Categories.AddAsync(category);
        //    await _context.SaveChangesAsync();

        //    return category;
        //}

        //public async Task<string> GetPrefixFromCategoryName(string categoryName)
        //{
        //    Category? category = await _context.Categories
        //        .FirstOrDefaultAsync(c => c.CateName.ToLower() == categoryName.ToLower());

        //    return category?.Prefix ?? throw new ArgumentException($"Category with name '{categoryName}' not found");
        //}

        //public async Task<bool> CheckPrefixExistsAsync(string prefix)
        //{
        //    return await _context.Categories
        //        .AnyAsync(c => c.Prefix.ToLower() == prefix.ToLower());
        //}

        //public async Task UpdateCategoryAsync(Category category)
        //{
        //    _context.Entry(category).State = EntityState.Modified;
        //    await _context.SaveChangesAsync();
        //}

        //public async Task DeleteCategoryAsync(int categoryId)
        //{
        //    var category = await _context.Categories.FindAsync(categoryId);
        //    if (category != null)
        //    {
        //        _context.Categories.Remove(category);
        //        await _context.SaveChangesAsync();
        //    }
        //}
    }
}
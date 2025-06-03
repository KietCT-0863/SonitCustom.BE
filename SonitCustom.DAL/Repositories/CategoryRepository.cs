using Microsoft.EntityFrameworkCore;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;

namespace SonitCustom.DAL.Repositories
{
    /// <summary>
    /// Cài đặt các thao tác với Category trong cơ sở dữ liệu
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SonitCustomDBContext _context;

        /// <summary>
        /// Khởi tạo đối tượng CategoryRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public CategoryRepository(SonitCustomDBContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        /// <inheritdoc />
        public async Task CreateCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Category?> GetCategoryByNameAsync(string cateName)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CateName == cateName);
        }

        /// <inheritdoc />
        public async Task<bool> CheckPrefixExistsAsync(string prefix)
        {
            return await _context.Categories
                .AnyAsync(c => c.Prefix.ToLower() == prefix.ToLower());
        }

        /// <inheritdoc />
        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Category?> GetCategoryByIdAsync(int? id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CateId == id);
        }

        /// <inheritdoc />
        public async Task<bool> IsCategoryExistAsync(string cateName)
        {
            return await _context.Categories
                .AnyAsync(c => c.CateName.ToLower() == cateName.ToLower());
        }

        /// <inheritdoc />
        public async Task DeleteCategoryAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;

namespace SonitCustom.DAL.Repositories
{
    /// <summary>
    /// Cài đặt các thao tác với Product trong cơ sở dữ liệu
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly SonitCustomDBContext _context;

        /// <summary>
        /// Khởi tạo đối tượng ProductRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public ProductRepository(SonitCustomDBContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.CategoryNavigation)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<List<Product>> GetProductsByPrefixIdAsync(string prefix)
        {
            return await _context.Products
                .Where(p => p.ProdId.StartsWith(prefix))
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<Product?> GetProductByProIdAsync(string proId)
        {
            return await _context.Products
                .Include(p => p.CategoryNavigation)
                .FirstOrDefaultAsync(p => p.ProdId == proId);
        }

        /// <inheritdoc />
        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<List<Product>> GetAllProductOfCategoryAsync(int cateId)
        {
            return await _context.Products.Where(p => p.CategoryNavigation.CateId == cateId).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetProductCountByCategoryIdAsync(int cateId)
        {
            return await _context.Products.CountAsync(p => p.CategoryNavigation.CateId == cateId);
        }
    }
}
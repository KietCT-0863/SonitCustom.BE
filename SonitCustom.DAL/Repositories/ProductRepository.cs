using Microsoft.EntityFrameworkCore;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;

namespace SonitCustom.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SonitCustomDBContext _context;

        public ProductRepository(SonitCustomDBContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.CategoryNavigation)
                .ToListAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProductsByPrefixIdAsync(string prefix)
        {
            return await _context.Products
                .Where(p => p.ProdId.StartsWith(prefix))
                .ToListAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetProductByProIdAsync(string proId)
        {
            return await _context.Products
                .Include(p => p.CategoryNavigation)
                .FirstOrDefaultAsync(p => p.ProdId == proId);
        }

        public async Task DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        //public async Task<List<Product>> GetAllProductOfCategoryAsync(int cateId)
        //{
        //    return await _context.Products.Where(p => p.CategoryNavigation.CateId == cateId).ToListAsync();
        //}
    }
}
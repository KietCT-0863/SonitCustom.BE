using Microsoft.EntityFrameworkCore;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SonitCustom.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly SonitCustomDBContext _context;

        public ProductRepository(SonitCustomDBContext context)
        {
            _context = context;
        }

        // Get all products
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.CategoryNavigation)
                .ToListAsync();
        }

        // Get product by ID
        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        // Create new product
        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // Update existing product
        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return product;
        }

        // Delete product
        public async Task<bool> DeleteProductAsync(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetNumberOfProductByCategoryAsync(string category)
        {
            return await _context.Products
                .Where(p => p.CategoryNavigation.CateName == category)
                .CountAsync();
        }
    }
} 
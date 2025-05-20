using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SonitCustom.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Get all products
        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            try
            {
                List<Product> products = await _productRepository.GetAllProductsAsync();

                return products.Select(p => new ProductDTO
                {
                    ProId = p.ProId,
                    ProName = p.ProName,
                    Description = p.Description,
                    Price = p.Price,
                    ImgUrl = p.ImgUrl,
                    Category = p.CategoryNavigation?.CateName ?? "Uncategorized"
                }).ToList();
            }
            catch (Exception ex)
            {
                // Log error here
                throw new Exception("Error occurred while fetching products", ex);
            }
        }

        //// Get product by ID
        //public async Task<ProductDTO> GetProductByIdAsync(string id)
        //{
        //    try
        //    {
        //        var product = await _productRepository.GetProductByIdAsync(id);
        //        if (product == null)
        //        {
        //            throw new Exception($"Product with ID {id} not found");
        //        }
        //        return product;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error here
        //        throw new Exception($"Error occurred while fetching product with ID {id}", ex);
        //    }
        //}

        // Create new product
        public async Task<bool> CreateProductAsync(CreateProductDTO product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
                }

                // Add any business logic validation here
                if (string.IsNullOrEmpty(product.ProName))
                {
                    throw new Exception("Product name is required");
                }

                var proId = await GenerateProductId(product.Category);
                var proCate = await _categoryRepository.GetCategoryIdByNameAsync(product.Category);

                Product newProduct = new()
                {
                    ProId = proId,
                    ProName = product.ProName,
                    Description = product.Description,
                    Price = product.Price,
                    ImgUrl = product.ImgUrl,
                    Category = proCate
                };

                var createdProduct = await _productRepository.CreateProductAsync(newProduct);
                return true;
            }
            catch (Exception ex)
            {
                // Log error here
                throw new Exception("Error occurred while creating product", ex);
            }
        }

        private async Task<string> GenerateProductId(string category)
        {
            var products = await _productRepository.GetProductsByCategoryAsync(category);
            string prefix = category.ToLower() switch
            {
                "extender" => "ext",
                "joint protector" => "jp",
                "butt protector" => "bp",
                _ => throw new ArgumentException("Invalid category")
            };

            if (!products.Any())
            {
                return $"{prefix}001";
            }

            var maxNumber = products
                .Select(p => p.ProId)
                .Where(id => id.StartsWith(prefix))
                .Select(id => int.Parse(id.Substring(prefix.Length)))
                .DefaultIfEmpty(0)
                .Max();

            return $"{prefix}{(maxNumber + 1):D3}";
        }

        //// Update existing product
        //public async Task<ProductDTO> UpdateProductAsync(ProductDTO product)
        //{
        //    try
        //    {
        //        if (product == null)
        //        {
        //            throw new ArgumentNullException(nameof(product));
        //        }

        //        // Check if product exists
        //        var exists = await _productRepository.ProductExistsAsync(product.ProId);
        //        if (!exists)
        //        {
        //            throw new Exception($"Product with ID {product.ProId} not found");
        //        }

        //        // Add any business logic validation here
        //        if (string.IsNullOrEmpty(product.ProName))
        //        {
        //            throw new Exception("Product name is required");
        //        }

        //        return await _productRepository.UpdateProductAsync(product);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error here
        //        throw new Exception($"Error occurred while updating product with ID {product?.ProId}", ex);
        //    }
        //}

        //// Delete product
        //public async Task<bool> DeleteProductAsync(string id)
        //{
        //    try
        //    {
        //        // Check if product exists
        //        var exists = await _productRepository.ProductExistsAsync(id);
        //        if (!exists)
        //        {
        //            throw new Exception($"Product with ID {id} not found");
        //        }

        //        return await _productRepository.DeleteProductAsync(id);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log error here
        //        throw new Exception($"Error occurred while deleting product with ID {id}", ex);
        //    }
        //}
    }
} 
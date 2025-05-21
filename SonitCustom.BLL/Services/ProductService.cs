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

        // Get product by ID
        public async Task<ProductDTO> GetProductByIdAsync(string id)
        {
            try
            {
                Product product = await _productRepository.GetProductByIdAsync(id);
                if (product == null)
                {
                    throw new Exception($"Product {id} không tồn tại");
                }

                return new ProductDTO()
                {
                    ProId = product.ProId,
                    ProName = product.ProName,
                    Description = product.Description,
                    Price = product.Price,
                    ImgUrl = product.ImgUrl,
                    Category = product.CategoryNavigation.CateName
                };
            }
            catch (Exception ex)
            {
                // Log error here
                throw new Exception($"Error occurred while fetching product with ID {id}", ex);
            }
        }

        // Create new product
        public async Task<bool> CreateProductAsync(CreateProductDTO product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
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
            int numberOfProducts = await _productRepository.GetNumberOfProductByCategoryAsync(category);
            string prefix = await _categoryRepository.GetPrefixFromCategoryName(category);

            return $"{prefix}{(numberOfProducts + 1):D3}";
        }

        // Update existing product
        public async Task<Product> UpdateProductAsync(string id, UpdateProductDTO product)
        {
            try
            {
                if (product == null)
                {
                    throw new ArgumentNullException(nameof(product));
                }

                // Check if product exists
                Product existProduct = await _productRepository.GetProductByIdAsync(id);
                if (existProduct == null)
                {
                    throw new Exception($"Product {id} không tồn tại");
                }

                existProduct.ProName = string.IsNullOrEmpty(product.ProName) ? existProduct.ProName : product.ProName;
                existProduct.Description = string.IsNullOrEmpty(product.Description) ? existProduct.Description : product.Description;
                existProduct.ImgUrl = string.IsNullOrEmpty(product.ImgUrl) ? existProduct.ImgUrl : product.ImgUrl;
                existProduct.Price = string.IsNullOrEmpty(product.Price) ? existProduct.Price : product.Price;

                return await _productRepository.UpdateProductAsync(existProduct);
            }
            catch (Exception ex)
            {
                // Log error here
                throw new Exception($"Error occurred while updating product with ID {id}", ex);
            }
        }

        // Delete product
        public async Task<bool> DeleteProductAsync(string id)
        {
            try
            {
                // Check if product exists
                Product existProduct = await _productRepository.GetProductByIdAsync(id);
                if (existProduct == null)
                {
                    throw new Exception($"Product with ID {id} not found");
                }

                return await _productRepository.DeleteProductAsync(id);
            }
            catch (Exception ex)
            {
                // Log error here
                throw new Exception($"Error occurred while deleting product with ID {id}", ex);
            }
        }
    }
}
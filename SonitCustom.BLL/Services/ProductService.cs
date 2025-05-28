using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using SonitCustom.BLL.Exceptions;
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

        //public async Task<List<ProductDTO>> GetAllProductsAsync()
        //{
        //    try
        //    {
        //        List<Product> products = await _productRepository.GetAllProductsAsync();

        //        return products.Select(p => new ProductDTO
        //        {
        //            ProdId = p.ProdId,
        //            ProName = p.ProName,
        //            Description = p.Description,
        //            Price = p.Price,
        //            ImgUrl = p.ImgUrl,
        //            Category = p.CategoryNavigation?.CateName ?? "Uncategorized"
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error occurred while fetching products", ex);
        //    }
        //}

        //public async Task<ProductDTO> GetProductByIdAsync(string id)
        //{
        //    try
        //    {
        //        Product product = await _productRepository.GetProductByIdAsync(id);
        //        if (product == null)
        //        {
        //            throw new ProductNotFoundException(id);
        //        }

        //        return new ProductDTO()
        //        {
        //            ProdId = product.ProdId,
        //            ProName = product.ProName,
        //            Description = product.Description,
        //            Price = product.Price,
        //            ImgUrl = product.ImgUrl,
        //            Category = product.CategoryNavigation.CateName
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error occurred while fetching product with ID {id}", ex);
        //    }
        //}

        //public async Task CreateProductAsync(CreateProductDTO product)
        //{
        //    try
        //    {
        //        if (product == null)
        //        {
        //            throw new ArgumentNullException(nameof(product));
        //        }

        //        string proId = await GenerateProductId(product.Category);
        //        int proCate = await _categoryRepository.GetCategoryIdByNameAsync(product.Category);

        //        Product newProduct = new()
        //        {
        //            ProdId = proId,
        //            ProName = product.ProName,
        //            Description = product.Description,
        //            Price = product.Price,
        //            ImgUrl = product.ImgUrl,
        //            Category = proCate
        //        };

        //        await _productRepository.CreateProductAsync(newProduct);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error occurred while creating product", ex);
        //    }
        //}

        //private async Task<string> GenerateProductId(string category)
        //{
        //    int numberOfProducts = await _productRepository.GetNumberOfProductByCategoryAsync(category);
        //    string prefix = await _categoryRepository.GetPrefixFromCategoryName(category);

        //    return $"{prefix}{(numberOfProducts + 1):D3}";
        //}

        //public async Task<bool> UpdateProductAsync(int id, UpdateProductDTO product)
        //{
        //    try
        //    {
        //        if (product == null)
        //        {
        //            throw new ArgumentNullException(nameof(product));
        //        }

        //        Product existProduct = await _productRepository.GetProductByIdAsync(id);
        //        if (existProduct == null)
        //        {
        //            throw new ProductNotFoundException(id);
        //        }

        //        if (!string.IsNullOrEmpty(product.Category))
        //        {
        //            await UpdateProductHaveCategoryAsync(existProduct, product);

        //            // SAU NÀY KHI MỞ RỘNG PROJECT, KHI GỌI HÀM UPDATE MÀ XẢY RA SỰ THAY ĐỔI ID CỦA PRODUCT, THÌ PHẢI ĐỒNG BỘ LẠI ID CỦA PRODUCT ĐÓ Ở NHỮNG TABLE KHÁC ĐỂ TRÁNH VIỆC XUNG ĐỘT DỮ LIỆU
        //            // *** RẤT QUAN TRỌNG ***
        //        }
        //        else
        //        {
        //            await UpdateProductHaveNoOrSameCategoryAsync(existProduct, product);
        //        }

        //        return true;
        //    }
        //    catch (ProductNotFoundException)
        //    {
        //        throw;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error occurred while updating product with ID {id}", ex);
        //    }
        //}

        //private async Task UpdateProductHaveCategoryAsync(Product existProduct, UpdateProductDTO product)
        //{
        //    if (string.IsNullOrEmpty(product.Category))
        //    {
        //        return;
        //    }

        //    int newCategoryId = await _categoryRepository.GetCategoryIdByNameAsync(product.Category);
            
        //    if (existProduct.Category == newCategoryId && existProduct.ProdId.StartsWith(existProduct.CategoryNavigation.Prefix, StringComparison.OrdinalIgnoreCase))
        //    {
        //        await UpdateProductHaveNoOrSameCategoryAsync(existProduct, product);
        //        return;
        //    }

        //    string newId = await GenerateProductId(product.Category);

        //    Product newProduct = new Product
        //    {
        //        ProdId = newId,
        //        ProName = !string.IsNullOrEmpty(product.ProName) ? product.ProName : existProduct.ProName,
        //        Description = !string.IsNullOrEmpty(product.Description) ? product.Description : existProduct.Description,
        //        Price = !string.IsNullOrEmpty(product.Price.ToString()) ? product.Price : existProduct.Price,
        //        ImgUrl = !string.IsNullOrEmpty(product.ImgUrl) ? product.ImgUrl : existProduct.ImgUrl,
        //        Category = newCategoryId
        //    };

        //    await _productRepository.DeleteProductAsync(existProduct);
        //    await _productRepository.CreateProductAsync(newProduct);
        //}
         
        //private async Task UpdateProductHaveNoOrSameCategoryAsync(Product existProduct, UpdateProductDTO product)
        //{
        //    existProduct.ProName = !string.IsNullOrEmpty(product.ProName) ? product.ProName : existProduct.ProName;
        //    existProduct.Description = !string.IsNullOrEmpty(product.Description) ? product.Description : existProduct.Description;
        //    existProduct.ImgUrl = !string.IsNullOrEmpty(product.ImgUrl) ? product.ImgUrl : existProduct.ImgUrl;
        //    existProduct.Price = !string.IsNullOrEmpty(product.Price.ToString()) ? product.Price : existProduct.Price;

        //    await _productRepository.UpdateProductAsync(existProduct);
        //}

        //public async Task<bool> DeleteProductAsync(string id)
        //{
        //    try
        //    {
        //        Product existProduct = await _productRepository.GetProductByIdAsync(id);

        //        if (existProduct == null)
        //        {
        //            throw new ProductNotFoundException(id);
        //        }

        //        await _productRepository.DeleteProductAsync(existProduct);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Error occurred while deleting product with ID {id}", ex);
        //    }
        //}
    }
}
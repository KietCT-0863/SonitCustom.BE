using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using System.Text.RegularExpressions;

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

        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            List<Product> products = await _productRepository.GetAllProductsAsync();

            return products.Select(p => new ProductDTO
            {
                ProdId = p.ProdId,
                ProName = p.ProName,
                Description = p.Description,
                Price = p.Price,
                ImgUrl = p.ImgUrl,
                Category = p.CategoryNavigation?.CateName ?? "Uncategorized",
                IsCustom = p.IsCustom,
            }).ToList();
        }



        public async Task CreateProductAsync(CreateProductDTO product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            Category? currentCate = await _categoryRepository.GetCategoryByNameAsync(product.Category);
            if (currentCate == null)
            {
                throw new CategoryNotFoundException(product.Category);
            }

            string proId = await GenerateProductId(currentCate);

            Product newProduct = new()
            {
                ProdId = proId,
                ProName = product.ProName,
                Description = product.Description,
                Price = product.Price,
                ImgUrl = product.ImgUrl,
                Category = currentCate.CateId,
                IsCustom = product.Price == 0 ? true : false,
            };

            await _productRepository.CreateProductAsync(newProduct);
        }

        private async Task<string> GenerateProductId(Category category)
        {
            List<Product> products = await _productRepository.GetProductsByPrefixIdAsync(category.Prefix);

            List<int> existingNumbers = new List<int>();
            string pattern = $@"{Regex.Escape(category.Prefix)}(\d+)";
            Regex regex = new Regex(pattern);

            foreach (var product in products)
            {
                Match match = regex.Match(product.ProdId);
                if (match.Success && match.Groups.Count > 1)
                {
                    if (int.TryParse(match.Groups[1].Value, out int number))
                    {
                        existingNumbers.Add(number);
                    }
                }
            }

            int nextNumber = 1;
            existingNumbers.Sort();

            foreach (int num in existingNumbers)
            {
                if (num == nextNumber)
                {
                    nextNumber++;
                }
                else if (num > nextNumber)
                {
                    break;
                }
            }

            return $"{category.Prefix}{nextNumber:D3}";
        }

        public async Task UpdateProductAsync(string proId, UpdateProductDTO updateProduct)
        {
            if (updateProduct == null)
            {
                throw new ArgumentNullException(nameof(updateProduct));
            }

            Product? existProduct = await _productRepository.GetProductByProIdAsync(proId);

            if (existProduct == null)
            {
                throw new ProductNotFoundException(proId);
            }

            existProduct = ProcessNormalInformationBeforeUpdate(existProduct, updateProduct);
            existProduct = await ProcessSpecalInformationBeforeUpdate(existProduct, updateProduct);
            existProduct = ProcessBusinessInformationBeforeUpdate(existProduct, updateProduct);

            await _productRepository.UpdateProductAsync(existProduct);
        }

        private Product ProcessNormalInformationBeforeUpdate(Product existProduct, UpdateProductDTO updateProduct)
        {
            existProduct.ProName = !string.IsNullOrEmpty(updateProduct.ProName) ? updateProduct.ProName : existProduct.ProName;
            existProduct.Description = !string.IsNullOrEmpty(updateProduct.Description) ? updateProduct.Description : existProduct.Description;
            existProduct.ImgUrl = !string.IsNullOrEmpty(updateProduct.ImgUrl) ? updateProduct.ImgUrl : existProduct.ImgUrl;

            return existProduct;
        }

        private async Task<Product> ProcessSpecalInformationBeforeUpdate(Product existProduct, UpdateProductDTO updateProduct)
        {
            if (!string.IsNullOrEmpty(updateProduct.Category) && existProduct.CategoryNavigation.CateName != updateProduct.Category)
            {
                Category? categoryUpdate = await _categoryRepository.GetCategoryByNameAsync(updateProduct.Category);
                if (categoryUpdate == null)
                {
                    throw new CategoryNotFoundException(updateProduct.Category);
                }

                existProduct.Category = categoryUpdate.CateId;
                existProduct.ProdId = await GenerateProductId(categoryUpdate);
            }

            return existProduct;
        }

        private Product ProcessBusinessInformationBeforeUpdate(Product existProduct, UpdateProductDTO updateProduct)
        {
            if (updateProduct.Price == null)
            {
                if (updateProduct.IsCustom != null)
                {
                    existProduct.IsCustom = updateProduct.IsCustom.Value;
                }
            }
            else if (updateProduct.Price > 0)
            {
                existProduct.Price = updateProduct.Price.Value;

                if (updateProduct.IsCustom != null)
                {
                    existProduct.IsCustom = updateProduct.IsCustom.Value;
                }
            }
            else
            {
                existProduct.Price = 0;
                existProduct.IsCustom = true;
            }

            return existProduct;
        }

        public async Task DeleteProductAsync(string prodId)
        {
            Product? existProduct = await _productRepository.GetProductByProIdAsync(prodId);

            if (existProduct == null)
            {
                throw new ProductNotFoundException(prodId);
            }

            await _productRepository.DeleteProductAsync(existProduct);
        }

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
    }
}
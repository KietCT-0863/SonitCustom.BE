using SonitCustom.BLL.DTOs.Products;
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

            return products.Select(MapProductToDTO).ToList();
        }

        public async Task CreateProductAsync(CreateProductDTO productDto)
        {
            ValidateProduct(productDto);

            Category? category = await GetAndValidateCategoryAsync(productDto.Category);

            string productId = await GenerateProductId(category);

            Product newProduct = new()
            {
                ProdId = productId,
                ProName = productDto.ProName,
                Description = productDto.Description,
                Price = productDto.Price,
                ImgUrl = productDto.ImgUrl,
                Category = category.CateId,
                IsCustom = productDto.Price == 0
            };

            await _productRepository.CreateProductAsync(newProduct);
        }

        public async Task UpdateProductAsync(string proId, UpdateProductDTO updateProduct)
        {
            ValidateUpdateProduct(updateProduct);

            Product? existProduct = await GetAndValidateProductExistsAsync(proId);

            UpdateBasicProductInfo(existProduct, updateProduct);

            await UpdateProductCategoryAndIdAsync(existProduct, updateProduct);

            UpdateBusinessInformation(existProduct, updateProduct);

            await _productRepository.UpdateProductAsync(existProduct);
        }

        public async Task DeleteProductAsync(string prodId)
        {
            Product? existProduct = await GetAndValidateProductExistsAsync(prodId);
            await _productRepository.DeleteProductAsync(existProduct);
        }

        public async Task RegenerateProductIdAfterCategoryUpdate(string oldProductId, Category updatedCategory)
        {
            Product? product = await GetAndValidateProductExistsAsync(oldProductId);
            string newProdId = await GenerateProductId(updatedCategory);
            product.ProdId = newProdId;

            await _productRepository.UpdateProductAsync(product);
        }

        private ProductDTO MapProductToDTO(Product product)
        {
            return new ProductDTO
            {
                ProdId = product.ProdId,
                ProName = product.ProName,
                Description = product.Description,
                Price = product.Price,
                ImgUrl = product.ImgUrl,
                Category = product.CategoryNavigation.CateName,
                IsCustom = product.IsCustom
            };
        }

        private void ValidateProduct(CreateProductDTO product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
        }

        private void ValidateUpdateProduct(UpdateProductDTO product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
        }

        private async Task<Product> GetAndValidateProductExistsAsync(string productId)
        {
            Product? product = await _productRepository.GetProductByProIdAsync(productId);

            if (product == null)
            {
                throw new ProductNotFoundException(productId);
            }

            return product;
        }

        private async Task<Category> GetAndValidateCategoryAsync(int? categoryId)
        {
            Category? category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            if (category == null)
            {
                throw new CategoryNotFoundException(categoryId);
            }

            return category;
        }

        private void UpdateBasicProductInfo(Product product, UpdateProductDTO updateData)
        {
            if (!string.IsNullOrEmpty(updateData.ProName))
            {
                product.ProName = updateData.ProName;
            }

            if (!string.IsNullOrEmpty(updateData.Description))
            {
                product.Description = updateData.Description;
            }

            if (!string.IsNullOrEmpty(updateData.ImgUrl))
            {
                product.ImgUrl = updateData.ImgUrl;
            }
        }

        private async Task UpdateProductCategoryAndIdAsync(Product product, UpdateProductDTO updateData)
        {
            if (updateData.Category != null && product.Category != updateData.Category)
            {
                Category? categoryUpdate = await GetAndValidateCategoryAsync(updateData.Category);
                product.Category = categoryUpdate.CateId;
                product.ProdId = await GenerateProductId(categoryUpdate);
            }
        }

        private void UpdateBusinessInformation(Product product, UpdateProductDTO updateData)
        {
            if (updateData.Price == null)
            {
                if (updateData.IsCustom != null)
                {
                    product.IsCustom = updateData.IsCustom.Value;
                }
            }
            else if (updateData.Price > 0)
            {
                product.Price = updateData.Price.Value;

                if (updateData.IsCustom != null)
                {
                    product.IsCustom = updateData.IsCustom.Value;
                }
            }
            else
            {
                product.Price = 0;
                product.IsCustom = true;
            }
        }

        private async Task<string> GenerateProductId(Category category)
        {
            List<Product> products = await _productRepository.GetProductsByPrefixIdAsync(category.Prefix);

            List<int> existingNumbers = ExtractExistingProductNumbers(products, category.Prefix);

            int nextNumber = FindNextAvailableNumber(existingNumbers);

            return $"{category.Prefix}{nextNumber:D3}";
        }

        private List<int> ExtractExistingProductNumbers(List<Product> products, string prefix)
        {
            List<int> existingNumbers = new List<int>();
            string pattern = $@"{Regex.Escape(prefix)}(\d+)";
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

            return existingNumbers;
        }

        private int FindNextAvailableNumber(List<int> existingNumbers)
        {
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

            return nextNumber;
        }
    }
}
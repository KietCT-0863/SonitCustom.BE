using Microsoft.AspNetCore.Http;
using SonitCustom.BLL.DTOs.Products;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using System.Text.RegularExpressions;

namespace SonitCustom.BLL.Services
{
    /// <summary>
    /// Service triển khai các thao tác liên quan đến sản phẩm
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IR2Service _r2Service;

        /// <summary>
        /// Khởi tạo đối tượng ProductService
        /// </summary>
        /// <param name="productRepository">Repository truy vấn dữ liệu sản phẩm</param>
        /// <param name="categoryRepository">Repository truy vấn dữ liệu category</param>
        /// <param name="r2Service">Service quản lý lưu trữ hình ảnh</param>
        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IR2Service r2Service)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _r2Service = r2Service;
        }

        /// <inheritdoc />
        public async Task<List<ProductDTO>> GetAllProductsAsync()
        {
            List<Product> products = await _productRepository.GetAllProductsAsync();

            return products.Select(MapProductToDTO).ToList();
        }

        /// <inheritdoc />
        public async Task CreateProductWithImageAsync(CreateProductDataDTO productData, IFormFile productImage)
        {
            string imageUrl = await _r2Service.UploadFileAsync(productImage);

            CreateProductDTO productDto = DataAndImageMapToCreateProductDTO(productData, imageUrl);
            await CreateProductAsync(productDto);
        }

        /// <summary>
        /// Tạo mới sản phẩm từ dữ liệu DTO
        /// </summary>
        /// <param name="productDto">Dữ liệu sản phẩm cần tạo</param>
        private async Task CreateProductAsync(CreateProductDTO productDto)
        {
            ValidateProduct(productDto);

            Category? category = await GetAndValidateCategoryAsync(productDto.Category);
            string productId = await GenerateProductId(category);

            Product newProduct = CreateProductDtoMapToProductEntity(productDto, productId, category.CateId);
            await _productRepository.CreateProductAsync(newProduct);
        }

        /// <inheritdoc />
        public async Task UpdateProductWithImageAsync(string prodId, UpdateProductDTO productData, UpdateProductImageDTO? imageData)
        {
            ValidateUpdateProduct(productData);
            Product? existingProduct = await GetAndValidateProductExistsAsync(prodId);

            if (imageData != null && imageData.ProductImage != null && !string.IsNullOrEmpty(existingProduct.ImgUrl))
            {
                productData.ImgUrl = await UpdateProductImageAsync(existingProduct.ImgUrl, imageData.ProductImage);
            }

            await UpdateProductAsync(prodId, productData);
        }

        /// <summary>
        /// Cập nhật hình ảnh sản phẩm
        /// </summary>
        /// <param name="oldImageUrl">URL hình ảnh cũ</param>
        /// <param name="newImage">Hình ảnh mới</param>
        /// <returns>URL hình ảnh mới</returns>
        private async Task<string> UpdateProductImageAsync(string oldImageUrl, IFormFile newImage)
        {
            string oldImageKey = ExtractImageKey(oldImageUrl);
            string newImageUrl = await _r2Service.UploadFileAsync(newImage);
            await _r2Service.DeleteFileAsync(oldImageKey);
            return newImageUrl;
        }

        /// <summary>
        /// Trích xuất khóa hình ảnh từ URL
        /// </summary>
        /// <param name="imageUrl">URL hình ảnh</param>
        /// <returns>Khóa hình ảnh</returns>
        private string ExtractImageKey(string imageUrl)
        {
            return imageUrl.Substring(imageUrl.LastIndexOf('/') + 1);
        }

        /// <summary>
        /// Cập nhật thông tin sản phẩm
        /// </summary>
        /// <param name="proId">Mã sản phẩm cần cập nhật</param>
        /// <param name="updateProduct">Dữ liệu cập nhật</param>
        private async Task UpdateProductAsync(string proId, UpdateProductDTO updateProduct)
        {
            ValidateUpdateProduct(updateProduct);
            Product existProduct = await GetAndValidateProductExistsAsync(proId);

            UpdateBasicProductInfo(existProduct, updateProduct);
            await UpdateProductCategoryAndIdAsync(existProduct, updateProduct);
            UpdateBusinessInformation(existProduct, updateProduct);

            await _productRepository.UpdateProductAsync(existProduct);
        }

        /// <inheritdoc />
        public async Task DeleteProductAsync(string prodId)
        {
            Product? existProduct = await GetAndValidateProductExistsAsync(prodId);

            await DeleteProductImageAsync(existProduct.ImgUrl);

            await _productRepository.DeleteProductAsync(existProduct);
        }

        /// <summary>
        /// Xóa hình ảnh sản phẩm
        /// </summary>
        /// <param name="imageUrl">URL hình ảnh cần xóa</param>
        private async Task DeleteProductImageAsync(string imageUrl)
        {
            string imageKey = ExtractImageKey(imageUrl);
            await _r2Service.DeleteFileAsync(imageKey);
        }

        /// <inheritdoc />
        public async Task RegenerateProductIdAfterCategoryUpdate(string oldProductId, Category updatedCategory)
        {
            Product? product = await GetAndValidateProductExistsAsync(oldProductId);
            string newProdId = await GenerateProductId(updatedCategory);
            product.ProdId = newProdId;

            await _productRepository.UpdateProductAsync(product);
        }

        /// <summary>
        /// Chuyển đổi đối tượng Product thành ProductDTO
        /// </summary>
        /// <param name="product">Đối tượng Product cần chuyển đổi</param>
        /// <returns>Đối tượng ProductDTO</returns>
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

        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu sản phẩm mới
        /// </summary>
        /// <param name="product">Đối tượng CreateProductDTO</param>
        /// <exception cref="ArgumentNullException">Ném ra khi dữ liệu trống</exception>
        private void ValidateProduct(CreateProductDTO product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu cập nhật sản phẩm
        /// </summary>
        /// <param name="product">Đối tượng UpdateProductDTO</param>
        /// <exception cref="ArgumentNullException">Ném ra khi dữ liệu trống</exception>
        private void ValidateUpdateProduct(UpdateProductDTO product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của sản phẩm theo mã
        /// </summary>
        /// <param name="productId">Mã sản phẩm cần kiểm tra</param>
        /// <returns>Đối tượng Product nếu tồn tại</returns>
        /// <exception cref="ProductNotFoundException">Ném ra khi không tìm thấy sản phẩm</exception>
        private async Task<Product> GetAndValidateProductExistsAsync(string productId)
        {
            Product? product = await _productRepository.GetProductByProIdAsync(productId);

            if (product == null)
            {
                throw new ProductNotFoundException(productId);
            }

            return product;
        }

        /// <summary>
        /// Lấy và kiểm tra sự tồn tại của category theo ID
        /// </summary>
        /// <param name="categoryId">ID của category cần kiểm tra</param>
        /// <returns>Đối tượng Category nếu tồn tại</returns>
        /// <exception cref="CategoryNotFoundException">Ném ra khi không tìm thấy category</exception>
        private async Task<Category> GetAndValidateCategoryAsync(int? categoryId)
        {
            Category? category = await _categoryRepository.GetCategoryByIdAsync(categoryId);

            if (category == null)
            {
                throw new CategoryNotFoundException(categoryId);
            }

            return category;
        }

        /// <summary>
        /// Cập nhật thông tin cơ bản của sản phẩm
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm cần cập nhật</param>
        /// <param name="updateData">Dữ liệu cập nhật</param>
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

        /// <summary>
        /// Cập nhật category và mã sản phẩm
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm cần cập nhật</param>
        /// <param name="updateData">Dữ liệu cập nhật</param>
        private async Task UpdateProductCategoryAndIdAsync(Product product, UpdateProductDTO updateData)
        {
            if (updateData.Category != null && product.Category != updateData.Category)
            {
                Category? categoryUpdate = await GetAndValidateCategoryAsync(updateData.Category);
                product.Category = categoryUpdate.CateId;
                product.ProdId = await GenerateProductId(categoryUpdate);
            }
        }

        /// <summary>
        /// Cập nhật thông tin kinh doanh của sản phẩm
        /// </summary>
        /// <param name="product">Đối tượng sản phẩm cần cập nhật</param>
        /// <param name="updateData">Dữ liệu cập nhật</param>
        private void UpdateBusinessInformation(Product product, UpdateProductDTO updateData)
        {
            if (updateData.Price == null)
            {
                if (updateData.IsCustom != null)
                {
                    product.IsCustom = updateData.IsCustom.Value;
                }
            }
            else
            {
                product.Price = updateData.Price.Value;
                product.IsCustom = false;
            }
        }

        /// <summary>
        /// Tạo mã sản phẩm mới dựa trên category
        /// </summary>
        /// <param name="category">Đối tượng category của sản phẩm</param>
        /// <returns>Mã sản phẩm mới</returns>
        private async Task<string> GenerateProductId(Category category)
        {
            List<Product> productsInCategory = await _productRepository.GetAllProductOfCategoryAsync(category.CateId);
            List<int> existingNumbers = ExtractExistingProductNumbers(productsInCategory, category.Prefix);
            int nextNumber = FindNextAvailableNumber(existingNumbers);

            return $"{category.Prefix}{nextNumber:D3}";
        }

        /// <summary>
        /// Trích xuất số thứ tự từ mã sản phẩm đã tồn tại
        /// </summary>
        /// <param name="products">Danh sách sản phẩm</param>
        /// <param name="prefix">Tiền tố của category</param>
        /// <returns>Danh sách số thứ tự đã tồn tại</returns>
        private List<int> ExtractExistingProductNumbers(List<Product> products, string prefix)
        {
            var numbers = new List<int>();
            string pattern = $@"{prefix}-(\d+)";
            var regex = new Regex(pattern);

            foreach (var product in products)
            {
                var match = regex.Match(product.ProdId);
                if (match.Success && match.Groups.Count > 1)
                {
                    if (int.TryParse(match.Groups[1].Value, out int number))
                    {
                        numbers.Add(number);
                    }
                }
            }
            return numbers;
        }

        /// <summary>
        /// Tìm số thứ tự tiếp theo chưa được sử dụng
        /// </summary>
        /// <param name="existingNumbers">Danh sách số thứ tự đã tồn tại</param>
        /// <returns>Số thứ tự tiếp theo</returns>
        private int FindNextAvailableNumber(List<int> existingNumbers)
        {
            if (existingNumbers.Count == 0)
            {
                return 1;
            }

            existingNumbers.Sort();
            int expectedNumber = 1;
            foreach (int number in existingNumbers)
            {
                if (expectedNumber < number)
                {
                    return expectedNumber;
                }
                expectedNumber = number + 1;
            }

            return expectedNumber;
        }

        /// <summary>
        /// Kết hợp dữ liệu sản phẩm và URL hình ảnh thành CreateProductDTO
        /// </summary>
        /// <param name="productData">Dữ liệu sản phẩm</param>
        /// <param name="imageUrl">URL hình ảnh</param>
        /// <returns>Đối tượng CreateProductDTO</returns>
        private CreateProductDTO DataAndImageMapToCreateProductDTO(CreateProductDataDTO productData, string imageUrl)
        {
            return new CreateProductDTO
            {
                ProName = productData.ProName,
                Description = productData.Description,
                Price = productData.Price,
                Category = productData.Category,
                ImgUrl = imageUrl,
                IsCustom = productData.IsCustom,
            };
        }

        /// <summary>
        /// Chuyển đổi CreateProductDTO thành đối tượng Product
        /// </summary>
        /// <param name="productDto">Dữ liệu DTO</param>
        /// <param name="productId">Mã sản phẩm đã tạo</param>
        /// <param name="categoryId">ID của category</param>
        /// <returns>Đối tượng Product</returns>
        private Product CreateProductDtoMapToProductEntity(CreateProductDTO productDto, string productId, int categoryId)
        {
            return new Product
            {
                ProdId = productId,
                ProName = productDto.ProName,
                Description = productDto.Description,
                Price = productDto.Price,
                ImgUrl = productDto.ImgUrl,
                Category = categoryId,
                IsCustom = productDto.IsCustom
            };
        }
    }
}
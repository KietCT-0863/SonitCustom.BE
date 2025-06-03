using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.DTOs.Categories;

namespace SonitCustom.BLL.Services
{
    /// <summary>
    /// Service triển khai các thao tác liên quan đến category sản phẩm
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        /// <summary>
        /// Khởi tạo đối tượng CategoryService
        /// </summary>
        /// <param name="categoryRepository">Repository truy vấn dữ liệu category</param>
        /// <param name="productRepository">Repository truy vấn dữ liệu sản phẩm</param>
        /// <param name="productService">Service xử lý các thao tác với sản phẩm</param>
        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository, IProductService productService)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _productService = productService;
        }

        /// <inheritdoc />
        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            List<Category> categories = await _categoryRepository.GetAllCategoriesAsync();

            return categories.Select(c => new CategoryDTO
            {
                CateId = c.CateId,
                CateName = c.CateName,
                Prefix = c.Prefix
            }).ToList();
        }

        /// <inheritdoc />
        public async Task CreateCategoryAsync(string cateName)
        {
            await ValidateCategoryNameAsync(cateName);

            string newPrefix = await GeneratePrefixFromCategoryNameAsync(cateName);

            Category category = new()
            {
                CateName = cateName,
                Prefix = newPrefix
            };

            await _categoryRepository.CreateCategoryAsync(category);
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của tên category
        /// </summary>
        /// <param name="cateName">Tên category cần kiểm tra</param>
        /// <exception cref="CategoryNameAlreadyExistsException">Ném ra khi tên category đã tồn tại</exception>
        private async Task ValidateCategoryNameAsync(string cateName)
        {
            Category? existCategory = await _categoryRepository.GetCategoryByNameAsync(cateName);

            if (existCategory != null)
            {
                throw new CategoryNameAlreadyExistsException(cateName);
            }
        }

        /// <inheritdoc />
        public async Task UpdateCategoryAsync(int cateId, UpdateCategoryDTO updateCategory)
        {
            Category? currentCate = await ValidateCategoryExistsAsync(cateId);
            await ValidateCategoryUpdateAsync(updateCategory, currentCate.CateName);

            currentCate = await ProcessCategoryUpdateAsync(currentCate, updateCategory);

            await _categoryRepository.UpdateCategoryAsync(currentCate);
            await RegenerateProductIdsForCategory(cateId, currentCate);
        }

        /// <inheritdoc />
        public async Task DeleteCategoryAsync(int cateId)
        {
            Category? currentCate = await ValidateCategoryExistsAsync(cateId);

            await ValidateCategoryHasNoProductsAsync(cateId);

            await _categoryRepository.DeleteCategoryAsync(currentCate);
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của category theo ID
        /// </summary>
        /// <param name="cateId">ID của category cần kiểm tra</param>
        /// <returns>Đối tượng Category nếu tồn tại</returns>
        /// <exception cref="CategoryNotFoundException">Ném ra khi không tìm thấy category</exception>
        private async Task<Category?> ValidateCategoryExistsAsync(int cateId)
        {
            Category? category = await _categoryRepository.GetCategoryByIdAsync(cateId);

            if (category == null)
            {
                throw new CategoryNotFoundException(cateId);
            }

            return category;
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của thông tin cập nhật category
        /// </summary>
        /// <param name="updateCategory">Dữ liệu cập nhật cho category</param>
        /// <param name="currentName">Tên hiện tại của category</param>
        /// <exception cref="CategoryNameAlreadyExistsException">Ném ra khi tên category mới đã tồn tại</exception>
        /// <exception cref="CategoryPrefixAlreadyExistsException">Ném ra khi mã tiền tố mới đã tồn tại</exception>
        private async Task ValidateCategoryUpdateAsync(UpdateCategoryDTO updateCategory, string currentName)
        {
            if (!string.IsNullOrEmpty(updateCategory.CateName) && updateCategory.CateName != currentName &&
                await _categoryRepository.IsCategoryExistAsync(updateCategory.CateName))
            {
                throw new CategoryNameAlreadyExistsException(updateCategory.CateName);
            }

            if (!string.IsNullOrEmpty(updateCategory.Prefix) &&
                await _categoryRepository.CheckPrefixExistsAsync(updateCategory.Prefix))
            {
                throw new CategoryPrefixAlreadyExistsException(updateCategory.Prefix);
            }
        }

        /// <summary>
        /// Xử lý cập nhật thông tin category
        /// </summary>
        /// <param name="category">Đối tượng category hiện tại</param>
        /// <param name="updateCategory">Dữ liệu cập nhật</param>
        /// <returns>Đối tượng category đã được cập nhật</returns>
        private async Task<Category> ProcessCategoryUpdateAsync(Category category, UpdateCategoryDTO updateCategory)
        {
            if (string.IsNullOrEmpty(updateCategory.Prefix))
            {
                if (!string.IsNullOrEmpty(updateCategory.CateName))
                {
                    category.CateName = updateCategory.CateName;
                    category.Prefix = await GeneratePrefixFromCategoryNameAsync(updateCategory.CateName);
                }
            }
            else
            {
                category.CateName = string.IsNullOrEmpty(updateCategory.CateName) ? category.CateName : updateCategory.CateName;
                category.Prefix = updateCategory.Prefix.ToLower();
            }

            return category;
        }

        /// <summary>
        /// Tạo mã tiền tố mới từ tên category
        /// </summary>
        /// <param name="category">Tên category</param>
        /// <returns>Mã tiền tố duy nhất</returns>
        private async Task<string> GeneratePrefixFromCategoryNameAsync(string category)
        {
            string suggestedPrefix = CreatePrefixFromCategoryName(category);
            return await GetUniquePrefixAsync(suggestedPrefix);
        }

        /// <summary>
        /// Tạo mã tiền tố ban đầu từ tên category
        /// </summary>
        /// <param name="category">Tên category</param>
        /// <returns>Mã tiền tố được đề xuất</returns>
        private string CreatePrefixFromCategoryName(string category)
        {
            string[] words = category.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 1)
            {
                return words[0].Length >= 3 ? words[0].Substring(0, 3) : words[0].PadRight(3, 'x');
            }

            return string.Join("", words.Select(w => w[0])).ToLower();
        }

        /// <summary>
        /// Đảm bảo mã tiền tố là duy nhất
        /// </summary>
        /// <param name="suggestedPrefix">Mã tiền tố đề xuất</param>
        /// <returns>Mã tiền tố duy nhất</returns>
        private async Task<string> GetUniquePrefixAsync(string suggestedPrefix)
        {
            bool prefixExists = await _categoryRepository.CheckPrefixExistsAsync(suggestedPrefix);
            return prefixExists ? "sonit" : suggestedPrefix;
        }

        /// <summary>
        /// Tạo lại mã sản phẩm cho tất cả sản phẩm thuộc category sau khi cập nhật
        /// </summary>
        /// <param name="categoryId">ID của category đã cập nhật</param>
        /// <param name="updatedCategory">Đối tượng category đã cập nhật</param>
        private async Task RegenerateProductIdsForCategory(int categoryId, Category updatedCategory)
        {
            List<Product> products = await _productRepository.GetAllProductOfCategoryAsync(categoryId);

            if (products == null)
            {
                return;
            }

            foreach (Product product in products)
            {
                await _productService.RegenerateProductIdAfterCategoryUpdate(product.ProdId, updatedCategory);
            }
        }

        /// <summary>
        /// Kiểm tra category không có sản phẩm nào trước khi xóa
        /// </summary>
        /// <param name="cateId">ID của category cần xóa</param>
        /// <exception cref="CategoryHasProductsException">Ném ra khi category vẫn còn sản phẩm</exception>
        private async Task ValidateCategoryHasNoProductsAsync(int cateId)
        {
            int productCount = await _productRepository.GetProductCountByCategoryIdAsync(cateId);
            if (productCount > 0)
            {
                throw new CategoryHasProductsException(cateId, productCount);
            }
        }
    }
}
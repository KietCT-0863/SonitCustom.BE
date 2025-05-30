using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.DTOs.Categories;

namespace SonitCustom.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public CategoryService(ICategoryRepository categoryRepository, IProductRepository productRepository, IProductService productService)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _productService = productService;
        }

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

        private async Task ValidateCategoryNameAsync(string cateName)
        {
            Category? existCategory = await _categoryRepository.GetCategoryByNameAsync(cateName);

            if (existCategory != null)
            {
                throw new CategoryNameAlreadyExistsException(cateName);
            }
        }

        public async Task UpdateCategoryAsync(int cateId, UpdateCategoryDTO updateCategory)
        {
            Category? currentCate = await ValidateCategoryExistsAsync(cateId);
            await ValidateCategoryUpdateAsync(updateCategory, currentCate.CateName);

            currentCate = await ProcessCategoryUpdateAsync(currentCate, updateCategory);

            await _categoryRepository.UpdateCategoryAsync(currentCate);
            await RegenerateProductIdsForCategory(cateId, currentCate);
        }

        public async Task DeleteCategoryAsync(int cateId)
        {
            Category? currentCate = await ValidateCategoryExistsAsync(cateId);
            await _categoryRepository.DeleteCategoryAsync(currentCate);
        }

        private async Task<Category?> ValidateCategoryExistsAsync(int cateId)
        {
            Category? category = await _categoryRepository.GetCategoryByIdAsync(cateId);

            if (category == null)
            {
                throw new CategoryNotFoundException(cateId);
            }

            return category;
        }

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
                category.Prefix = updateCategory.Prefix;
            }

            return category;
        }

        private async Task<string> GeneratePrefixFromCategoryNameAsync(string category)
        {
            string suggestedPrefix = CreatePrefixFromCategoryName(category);
            return await GetUniquePrefixAsync(suggestedPrefix);
        }

        private string CreatePrefixFromCategoryName(string category)
        {
            string[] words = category.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 1)
            {
                return words[0].Length >= 3 ? words[0].Substring(0, 3) : words[0].PadRight(3, 'x');
            }

            return string.Join("", words.Select(w => w[0]));
        }

        private async Task<string> GetUniquePrefixAsync(string suggestedPrefix)
        {
            bool prefixExists = await _categoryRepository.CheckPrefixExistsAsync(suggestedPrefix);
            return prefixExists ? "sonit" : suggestedPrefix;
        }

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
    }
}
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Exceptions;

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
            Category? existCategory = await _categoryRepository.GetCategoryByNameAsync(cateName);

            if (existCategory!=null)
            {
                throw new CategoryNameExistException(cateName);
            }

            string newPrefix = await GeneratePrefixFromCategory(cateName);

            Category category = new()
            {
                CateName = cateName,
                Prefix = newPrefix
            };

            await _categoryRepository.CreateCategoryAsync(category);
        }

        private async Task<string> GeneratePrefixFromCategory(string category)
        {
            string[] words = category.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string newPrefix;

            if (words.Length == 1)
            {
                newPrefix = words[0].Length >= 3 ? words[0].Substring(0, 3) : words[0].PadRight(3, 'x');
            }
            else
            {
                newPrefix = string.Join("", words.Select(w => w[0]));
            }

            bool prefixExists = await _categoryRepository.CheckPrefixExistsAsync(newPrefix);

            return prefixExists ? "sonit" : newPrefix;
        }

        public async Task UpdateCategoryAsync(int cateId, UpdateCategoryDTO categoryDTO)
        {
            Category? currentCate = await _categoryRepository.GetCategoryByIdAsync(cateId);

            if (currentCate == null)
            {
                throw new CategoryNotFoundException(cateId);
            }

            if (!string.IsNullOrEmpty(categoryDTO.CateName) && await _categoryRepository.IsCategoryExistAsync(categoryDTO.CateName))
            {
                throw new CategoryNameExistException(categoryDTO.CateName);
            }

            if (await _categoryRepository.CheckPrefixExistsAsync(categoryDTO.Prefix))
            {
                throw new CategoryPrefixExistException(categoryDTO.Prefix);
            }

            if (string.IsNullOrEmpty(categoryDTO.Prefix))
            {
                currentCate.CateName = categoryDTO.CateName;
                currentCate.Prefix = await GeneratePrefixFromCategory(categoryDTO.CateName);
            }
            else
            {
                currentCate.CateName = string.IsNullOrEmpty(categoryDTO.CateName) ? currentCate.CateName : categoryDTO.CateName;
                currentCate.Prefix = categoryDTO.Prefix;
            }

            await _categoryRepository.UpdateCategoryAsync(currentCate);

            //// Lấy danh sách tất cả sản phẩm thuộc category này
            //List<Product> products = await _productRepository.GetAllProductOfCategoryAsync(cateId);

            //if (products == null)
            //{
            //    return;
            //}

            //foreach (Product product in products)
            //{
            //    UpdateProductDTO updateProduct = new()
            //    {
            //        ProName = product.ProName,
            //        Description = product.Description,
            //        ImgUrl = product.ImgUrl,
            //        Price = product.Price,
            //        Category = currentCate.CateName
            //    };

            //    await _productService.UpdateProductAsync(product.Id, updateProduct);
        }

        public async Task DeleteCategoryAsync(int cateId)
        {
            Category? currentCate = await _categoryRepository.GetCategoryByIdAsync(cateId);

            if (currentCate == null)
            {
                throw new CategoryNotFoundException(cateId);
            }

            await _categoryRepository.DeleteCategoryAsync(currentCate);
        }
    }
}
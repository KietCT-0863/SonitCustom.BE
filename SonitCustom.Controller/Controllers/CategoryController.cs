using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Interface;
using System.Threading.Tasks;
using System.Collections.Generic;
using SonitCustom.Controller.Helpers;
using Microsoft.AspNetCore.Authorization;
using SonitCustom.BLL.Services;

namespace SonitCustom.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ITokenService _tokenService;

        public CategoryController(ICategoryService categoryService, ITokenService tokenService)
        {
            _categoryService = categoryService;
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                List<CategoryDTO> categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCategory([FromBody] string categoryName)
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
                bool result = await _categoryService.CreateCategoryAsync(categoryName);
                return Ok(new { message = "Tạo category thành công" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = $"Lỗi khi tạo category: {ex.Message}" });
            }
        }

    }
}
using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Interface;
using SonitCustom.Controller.Helpers;
using System.Threading.Tasks;

namespace SonitCustom.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                if (!JwtCookieHelper.IsAdmin(Request))
                {
                    return Unauthorized(new { message = "Chỉ admin mới có quyền truy cập" });
                }

                List<CategoryDTO> categories = await _categoryService.GetAllCategoriesAsync();
                return Ok(categories);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] string categoryName)
        {
            try
            {
                if (!JwtCookieHelper.IsAdmin(Request))
                {
                    return Unauthorized(new { message = "Chỉ admin mới có quyền truy cập" });
                }

                bool result = await _categoryService.CreateCategoryAsync(categoryName);
                return Ok("Tạo category thành công");
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
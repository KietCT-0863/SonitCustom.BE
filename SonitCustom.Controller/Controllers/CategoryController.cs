using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using SonitCustom.Controller.Helpers;
using Microsoft.AspNetCore.Authorization;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.DTOs.Categories;

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
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCategory([FromBody] string categoryName)
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
                await _categoryService.CreateCategoryAsync(categoryName);
                return Ok(new { message = "Tạo category thành công" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (CategoryNameAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO categoryDTO)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "ID category không hợp lệ" });
                }

                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
                await _categoryService.UpdateCategoryAsync(id, categoryDTO);
                return Ok(new { message = "Cập nhật category thành công" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (ArgumentNullException)
            {
                return BadRequest(new { message = "Dữ liệu cập nhật không hợp lệ" });
            }
            catch (CategoryNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (CategoryNameAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (CategoryPrefixAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "ID category không hợp lệ" });
                }

                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
                await _categoryService.DeleteCategoryAsync(id);
                return Ok(new { message = "Xóa category thành công" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (CategoryNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }
    }
}
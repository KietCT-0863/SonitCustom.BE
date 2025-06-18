using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.DTOs.Categories;

namespace SonitCustom.Controller.Controllers
{
    /// <summary>
    /// API controller để quản lý category sản phẩm
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Khởi tạo controller với các dependency cần thiết
        /// </summary>
        /// <param name="categoryService">Service xử lý logic category</param>
        /// <param name="tokenService">Service quản lý token xác thực</param>
        public CategoryController(ICategoryService categoryService, ITokenService tokenService)
        {
            _categoryService = categoryService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Lấy tất cả category sản phẩm
        /// </summary>
        /// <returns>Danh sách tất cả category</returns>
        /// <response code="200">Trả về danh sách category</response>
        /// <response code="500">Lỗi xảy ra khi truy xuất dữ liệu</response>
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

        /// <summary>
        /// Tạo mới một category sản phẩm
        /// </summary>
        /// <param name="createCategoryDTO">Thông tin category cần tạo</param>
        /// <returns>Thông báo kết quả tạo category</returns>
        /// <response code="200">Tạo category thành công</response>
        /// <response code="401">Người dùng không có quyền tạo category</response>
        /// <response code="409">Tên category đã tồn tại</response>
        /// <response code="500">Lỗi server</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDTO createCategoryDTO)
        {
            try
            {
                await _categoryService.CreateCategoryAsync(createCategoryDTO.CategoryName);
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

        /// <summary>
        /// Cập nhật thông tin của một category sản phẩm
        /// </summary>
        /// <param name="id">ID của category cần cập nhật</param>
        /// <param name="categoryDTO">Thông tin cần cập nhật</param>
        /// <returns>Thông báo kết quả cập nhật</returns>
        /// <response code="200">Cập nhật category thành công</response>
        /// <response code="400">Dữ liệu không hợp lệ</response>
        /// <response code="401">Người dùng không có quyền cập nhật category</response>
        /// <response code="404">Không tìm thấy category</response>
        /// <response code="409">Tên category hoặc tiền tố đã tồn tại</response>
        /// <response code="500">Lỗi server</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDTO categoryDTO)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest(new { message = "ID category không hợp lệ" });
                }

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

        /// <summary>
        /// Xóa một category sản phẩm
        /// </summary>
        /// <param name="id">ID của category cần xóa</param>
        /// <returns>Thông báo kết quả xóa category</returns>
        /// <response code="200">Xóa category thành công</response>
        /// <response code="400">ID không hợp lệ hoặc category đang được sử dụng bởi sản phẩm</response>
        /// <response code="401">Người dùng không có quyền xóa category</response>
        /// <response code="404">Không tìm thấy category</response>
        /// <response code="500">Lỗi server</response>
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
            catch (CategoryHasProductsException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }
    }
}
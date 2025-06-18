using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.DTOs.Roles;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using SonitCustom.BLL.Interface.Security;

namespace SonitCustom.Controller.Controllers
{
    /// <summary>
    /// API controller để quản lý role trong hệ thống
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Khởi tạo controller với các dependency cần thiết
        /// </summary>
        /// <param name="roleService">Service xử lý logic role</param>
        /// <param name="tokenService">Service quản lý token xác thực</param>
        public RoleController(IRoleService roleService, ITokenService tokenService)
        {
            _roleService = roleService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Lấy danh sách tất cả role trong hệ thống
        /// </summary>
        /// <returns>Danh sách thông tin role</returns>
        /// <response code="200">Trả về danh sách role</response>
        /// <response code="401">Người dùng không có quyền truy cập</response>
        /// <response code="500">Lỗi server</response>
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<List<RoleDTO>>> GetAllRoles()
        {
            try
            {
                List<RoleDTO> roles = await _roleService.GetAllRolesAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        //[HttpGet("{id}")]
        //[Authorize(Roles = "admin")]
        //public async Task<ActionResult<RoleDTO>> GetRoleById(int id)
        //{
        //    try
        //    {
        //        RoleDTO role = await _roleService.GetRoleByIdAsync(id);
        //        return Ok(role);
        //    }
        //    catch (RoleNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Lỗi server: {ex.Message}");
        //    }
        //}

        /// <summary>
        /// Tạo mới một role trong hệ thống
        /// </summary>
        /// <param name="createRoleDTO">Thông tin role cần tạo</param>
        /// <returns>Thông báo kết quả tạo role</returns>
        /// <response code="200">Tạo role thành công</response>
        /// <response code="401">Người dùng không có quyền truy cập</response>
        /// <response code="409">Tên role đã tồn tại</response>
        /// <response code="500">Lỗi server</response>
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateRole(CreateRoleDTO createRoleDTO)
        {
            try
            {
                await _roleService.CreateRoleAsync(createRoleDTO);
                return Ok(new { message = "Thêm role thành công" });
            }
            catch (RoleNameAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật thông tin của một role
        /// </summary>
        /// <param name="id">ID của role cần cập nhật</param>
        /// <param name="updateRoleDTO">Thông tin cần cập nhật</param>
        /// <returns>Thông báo kết quả cập nhật</returns>
        /// <response code="200">Cập nhật role thành công</response>
        /// <response code="401">Người dùng không có quyền truy cập</response>
        /// <response code="404">Không tìm thấy role</response>
        /// <response code="409">Tên role đã tồn tại</response>
        /// <response code="500">Lỗi server</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateRole(int id, UpdateRoleDTO updateRoleDTO)
        {
            try
            {
                await _roleService.UpdateRoleAsync(id, updateRoleDTO);
                return Ok(new { message = "Chỉnh sửa role thành công" });
            }
            catch (RoleNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RoleNameAlreadyExistsException ex)
            {
                return Conflict(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa một role trong hệ thống
        /// </summary>
        /// <param name="id">ID của role cần xóa</param>
        /// <returns>Thông báo kết quả xóa role</returns>
        /// <response code="200">Xóa role thành công</response>
        /// <response code="400">role đang được sử dụng bởi người dùng</response>
        /// <response code="401">Người dùng không có quyền truy cập</response>
        /// <response code="404">Không tìm thấy role</response>
        /// <response code="500">Lỗi server</response>
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await _roleService.DeleteRoleAsync(id);
                return Ok(new { message = "Xoá role thành công" });
            }
            catch (RoleNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (RoleHasUsersException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }
    }
} 
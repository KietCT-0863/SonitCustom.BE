using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.DTOs.Roles;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.Controller.Helpers;

namespace SonitCustom.Controller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ITokenService _tokenService;

        public RoleController(IRoleService roleService, ITokenService tokenService)
        {
            _roleService = roleService;
            _tokenService = tokenService;
        }

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

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDTO createRoleDTO)
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
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
            catch (InvalidRefreshTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] UpdateRoleDTO updateRoleDTO)
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
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
            catch (InvalidRefreshTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi server: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
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
            catch (InvalidRefreshTokenException ex)
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
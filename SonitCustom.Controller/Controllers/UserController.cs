using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using SonitCustom.Controller.Helpers;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);
                List<UserDTO> users = await _userService.GetAllUsersAsync();
                return Ok(users);
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
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                string? accessToken = CookieHelper.GetAccessToken(Request);
                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new UnauthorizedAccessException("Phiên đăng nhập không hợp lệ");
                }

                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);

                int userId = CookieHelper.GetUserIdFromToken(User);
                RespondUserDTO? user = await _userService.GetUserByIdAsync(userId);

                return Ok(user);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidRefreshTokenException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMe([FromBody] UpdateUserDTO updateUserDTO)
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);

                int userId = CookieHelper.GetUserIdFromToken(User);

                await _userService.UpdateUserAsync(userId, updateUserDTO);

                return Ok(new { message = "Cập nhật thông tin người dùng thành công" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DuplicateUserNameException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DuplicateEmailException ex)
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
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminUpdateUser(int id, [FromBody] AdminUpdateUserDTO adminUpdateUserDTO)
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);

                await _userService.AdminUpdateUserAsync(id, adminUpdateUserDTO);

                return Ok(new { message = "Cập nhật thông tin người dùng thành công" });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (RoleNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DuplicateUserNameException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DuplicateEmailException ex)
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
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpDelete("me")]
        [Authorize]
        public async Task<IActionResult> DeleteMe()
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);

                int userId = CookieHelper.GetUserIdFromToken(User);

                await _userService.DeleteAccountAsync(userId);

                string? refreshToken = CookieHelper.GetRefreshToken(Request);
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    await _tokenService.RevokeRefreshTokenAsync(refreshToken);
                }

                CookieHelper.RemoveAllAuthCookies(Response);

                return Ok(new { message = "Xóa tài khoản thành công" });
            }
            catch (AdminDeleteException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
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
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminDeleteUser(int id)
        {
            try
            {
                await CookieHelper.TryRefreshAccessToken(Request, Response, _tokenService);

                int adminId = CookieHelper.GetUserIdFromToken(User);

                await _userService.DeleteAccountAsync(id);
                
                await _tokenService.RevokeRefreshTokenByUserIdAsync(id);

                return Ok(new { message = "Xóa tài khoản người dùng thành công" });
            }
            catch (AdminDeleteException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
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
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using Microsoft.AspNetCore.Authorization;
using SonitCustom.Controller.Helpers;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.Controller.Controllers
{
    /// <summary>
    /// API controller để quản lý người dùng trong hệ thống
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Khởi tạo controller với các dependency cần thiết
        /// </summary>
        /// <param name="userService">Service xử lý logic người dùng</param>
        /// <param name="tokenService">Service quản lý token xác thực</param>
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Lấy danh sách tất cả người dùng trong hệ thống
        /// </summary>
        /// <returns>Danh sách thông tin người dùng</returns>
        /// <response code="200">Trả về danh sách người dùng</response>
        /// <response code="401">Người dùng không có quyền truy cập</response>
        /// <response code="500">Lỗi server</response>
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

        /// <summary>
        /// Lấy thông tin của người dùng hiện tại
        /// </summary>
        /// <returns>Thông tin chi tiết của người dùng đang đăng nhập</returns>
        /// <response code="200">Trả về thông tin người dùng</response>
        /// <response code="401">Phiên đăng nhập không hợp lệ hoặc đã hết hạn</response>
        /// <response code="404">Không tìm thấy thông tin người dùng</response>
        /// <response code="500">Lỗi server</response>
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

        /// <summary>
        /// Cập nhật thông tin của người dùng hiện tại
        /// </summary>
        /// <param name="updateUserDTO">Thông tin cần cập nhật</param>
        /// <returns>Thông báo kết quả cập nhật</returns>
        /// <response code="200">Cập nhật thành công</response>
        /// <response code="400">Dữ liệu không hợp lệ hoặc trùng lặp</response>
        /// <response code="401">Phiên đăng nhập không hợp lệ hoặc đã hết hạn</response>
        /// <response code="404">Không tìm thấy thông tin người dùng</response>
        /// <response code="500">Lỗi server</response>
        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMe([FromForm] UpdateUserDTO updateUserDTO)
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

        /// <summary>
        /// Cập nhật thông tin người dùng bởi quản trị viên
        /// </summary>
        /// <param name="id">ID của người dùng cần cập nhật</param>
        /// <param name="adminUpdateUserDTO">Thông tin cần cập nhật</param>
        /// <returns>Thông báo kết quả cập nhật</returns>
        /// <response code="200">Cập nhật thành công</response>
        /// <response code="400">Dữ liệu không hợp lệ hoặc trùng lặp</response>
        /// <response code="401">Người dùng không có quyền truy cập</response>
        /// <response code="404">Không tìm thấy thông tin người dùng hoặc role</response>
        /// <response code="500">Lỗi server</response>
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AdminUpdateUser(int id, [FromForm] AdminUpdateUserDTO adminUpdateUserDTO)
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

        /// <summary>
        /// Xóa tài khoản người dùng hiện tại
        /// </summary>
        /// <returns>Thông báo kết quả xóa tài khoản</returns>
        /// <response code="200">Xóa tài khoản thành công</response>
        /// <response code="400">Không thể xóa tài khoản quản trị viên</response>
        /// <response code="401">Phiên đăng nhập không hợp lệ hoặc đã hết hạn</response>
        /// <response code="404">Không tìm thấy thông tin người dùng</response>
        /// <response code="500">Lỗi server</response>
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

        /// <summary>
        /// Xóa tài khoản người dùng bởi quản trị viên
        /// </summary>
        /// <param name="id">ID của người dùng cần xóa</param>
        /// <returns>Thông báo kết quả xóa tài khoản</returns>
        /// <response code="200">Xóa tài khoản thành công</response>
        /// <response code="400">Không thể xóa tài khoản quản trị viên</response>
        /// <response code="401">Người dùng không có quyền truy cập</response>
        /// <response code="404">Không tìm thấy thông tin người dùng</response>
        /// <response code="500">Lỗi server</response>
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
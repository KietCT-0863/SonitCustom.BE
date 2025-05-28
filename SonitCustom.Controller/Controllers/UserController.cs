using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using SonitCustom.BLL.DTOs;
using Microsoft.AspNetCore.Authorization;
using SonitCustom.Controller.Helpers;

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
            catch (UnauthorizedAccessException)
            {
                return Unauthorized(new { message = "Bạn không có quyền sử dụng API này" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi lấy danh sách người dùng" });
            }
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            string? accessToken = CookieHelper.GetAccessToken(Request);
            if (string.IsNullOrEmpty(accessToken))
            {
                return Unauthorized(new { message = "Phiên đăng nhập không hợp lệ" });
            }

            int userId = int.Parse(User.FindFirst("userid")?.Value ?? "0");
            if (userId == 0)
            {
                return Unauthorized(new { message = "Token không hợp lệ" });
            }

            UserDTO? user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "Không tìm thấy người dùng" });
            }

            return Ok(user);
        }
    }
}
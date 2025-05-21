using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using System.Threading.Tasks;
using SonitCustom.Controller.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace SonitCustom.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!JwtCookieHelper.IsAdmin(Request))
                return Unauthorized(new { message = "Chỉ admin mới có quyền truy cập" });

            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/user/me
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userInfo = JwtCookieHelper.GetUserInfoFromCookie(Request);
            if (userInfo == null)
                return Unauthorized();

            var (userId, _) = userInfo.Value;
            var user = await _userService.GetUserByIdAsync(userId ?? -1);

            if (user == null)
                return NotFound(new { message = "Không tìm thấy người dùng" });
            return Ok(user);
        }
    }
} 
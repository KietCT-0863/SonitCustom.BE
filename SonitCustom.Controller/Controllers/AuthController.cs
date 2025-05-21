using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using SonitCustom.BLL.DTOs;
using SonitCustom.DAL.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using SonitCustom.Controller.Helpers;

namespace SonitCustom.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public AuthController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }    

            // Xóa cookie cũ nếu có
            Response.Cookies.Delete("jwt_token");

            UserDTO user = await _loginService.LoginAsync(loginModel.Username, loginModel.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không hợp lệ" });
            }    

            // Sinh JWT token
            var token = await _loginService.GenerateJwtTokenAsync(user);

            // Đặt token vào cookie
            JwtCookieHelper.SetJwtCookie(Response, token);

            // Trả về message thành công, không trả về user, không trả về token
            return Ok(new { message ="Đăng nhập thành công" });
        }
    }
} 
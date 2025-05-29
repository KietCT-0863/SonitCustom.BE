using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using SonitCustom.Controller.Helpers;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.DTOs.Auth;

namespace SonitCustom.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(ILoginService loginService, ITokenService tokenService, IUserService userService)
        {
            _loginService = loginService;
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            UserDTO? user = await _loginService.LoginAsync(request.Username, request.Password);

            if (user == null)
            {
                return Unauthorized(new { message = "Tên đăng nhập hoặc mật khẩu không đúng" });
            }

            AccessTokenDTO accessToken = _tokenService.GenerateAccessToken(user.Id, user.RoleName);
            RefreshTokenDTO refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            CookieHelper.SetAccessTokenCookie(Response, accessToken);
            CookieHelper.SetRefreshTokenCookie(Response, refreshToken);

            return Ok(new
            {
                message = "Đăng nhập thành công"
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            string? refreshToken = CookieHelper.GetRefreshToken(Request);
            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _tokenService.RevokeRefreshTokenAsync(refreshToken);
            }

            CookieHelper.RemoveAllAuthCookies(Response);

            return Ok(new
            {
                message = "Đăng xuất thành công"
            });
        }
    }
}
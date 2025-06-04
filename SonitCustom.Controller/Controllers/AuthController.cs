using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using SonitCustom.Controller.Helpers;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.Controller.Controllers
{
    /// <summary>
    /// API controller để quản lý xác thực người dùng
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Khởi tạo controller với các dependency cần thiết
        /// </summary>
        /// <param name="loginService">Service xử lý logic đăng nhập</param>
        /// <param name="tokenService">Service quản lý token xác thực</param>
        public AuthController(ILoginService loginService, ITokenService tokenService)
        {
            _loginService = loginService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Đăng nhập vào hệ thống
        /// </summary>
        /// <param name="request">Thông tin đăng nhập</param>
        /// <returns>Thông báo kết quả đăng nhập</returns>
        /// <response code="200">Đăng nhập thành công và trả về token xác thực</response>
        /// <response code="401">Thông tin đăng nhập không hợp lệ</response>
        /// <response code="500">Lỗi server</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            try
            {
                UserDTO user = await _loginService.LoginAsync(request.Username, request.Password);

                AccessTokenDTO accessToken = _tokenService.GenerateAccessToken(user.Id, user.RoleName);
                RefreshTokenDTO refreshToken = _tokenService.GenerateRefreshToken(user.Id);

                CookieHelper.SetAccessTokenCookie(Response, accessToken);
                CookieHelper.SetRefreshTokenCookie(Response, refreshToken);

                return Ok(new
                {
                    message = "Đăng nhập thành công"
                });
            }
            catch (InvalidCredentialsException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }

        /// <summary>
        /// Đăng xuất khỏi hệ thống
        /// </summary>
        /// <returns>Thông báo kết quả đăng xuất</returns>
        /// <response code="200">Đăng xuất thành công</response>
        /// <response code="500">Lỗi server</response>
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
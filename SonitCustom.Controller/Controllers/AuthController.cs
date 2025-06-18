using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
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
        /// <returns>Thông báo kết quả đăng nhập và tokens xác thực</returns>
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

                return Ok(new
                {
                    message = "Đăng nhập thành công",
                    accessToken = accessToken.Token,
                    refreshToken = refreshToken.Token,
                    expiresAt = accessToken.ExpiresAt
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
        /// Làm mới access token khi hết hạn
        /// </summary>
        /// <param name="request">Refresh token request</param>
        /// <returns>Access token mới</returns>
        /// <response code="200">Tạo access token mới thành công</response>
        /// <response code="401">Refresh token không hợp lệ</response>
        /// <response code="500">Lỗi server</response>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.RefreshToken))
                {
                    return Unauthorized(new { message = "Refresh token không được cung cấp" });
                }

                AccessTokenDTO accessToken = await _tokenService.RefreshAccessTokenAsync(request.RefreshToken);
                
                return Ok(new
                {
                    accessToken = accessToken.Token,
                    expiresAt = accessToken.ExpiresAt
                });
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
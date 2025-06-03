using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.Controller.Controllers
{
    /// <summary>
    /// API controller để xử lý đăng ký tài khoản mới
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        /// <summary>
        /// Khởi tạo controller với các dependency cần thiết
        /// </summary>
        /// <param name="registerService">Service xử lý logic đăng ký</param>
        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        /// <summary>
        /// Đăng ký tài khoản người dùng mới
        /// </summary>
        /// <param name="newRegister">Thông tin đăng ký tài khoản</param>
        /// <returns>Thông báo kết quả đăng ký</returns>
        /// <response code="200">Đăng ký thành công</response>
        /// <response code="400">Dữ liệu không hợp lệ hoặc đăng ký thất bại</response>
        /// <response code="409">Tên đăng nhập hoặc email đã tồn tại</response>
        /// <response code="500">Lỗi server</response>
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterUserDTO newRegister)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool createdUser = await _registerService.RegisterAsync(newRegister);
                if (!createdUser)
                {
                    return BadRequest(new { message = "Đăng ký thất bại" });
                }

                return Ok(new { message = "Đăng ký thành công" });
            }
            catch (UserCredentialsAlreadyExistsException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi hệ thống: {ex.Message}" });
            }
        }
    }
} 
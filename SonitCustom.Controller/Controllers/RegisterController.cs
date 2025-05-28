using Microsoft.AspNetCore.Mvc;
using SonitCustom.BLL.Interface;
using SonitCustom.BLL.DTOs;

namespace SonitCustom.Controller.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO newRegister)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool createdUser = await _registerService.RegisterAsync(newRegister);
                if (createdUser == null)
                {
                    return BadRequest(new { message = "Đăng ký thất bại" });
                }

                return Ok(new { message = "Đăng ký thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 
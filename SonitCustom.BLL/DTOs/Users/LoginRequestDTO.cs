using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs.Users
{
    /// <summary>
    /// DTO dùng để xác thực đăng nhập người dùng
    /// </summary>
    public class LoginRequestDTO
    {
        /// <summary>
        /// Tên đăng nhập của người dùng
        /// </summary>
        /// <remarks>Bắt buộc phải nhập</remarks>
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string Username { get; set; }

        /// <summary>
        /// Mật khẩu của người dùng
        /// </summary>
        /// <remarks>Bắt buộc phải nhập</remarks>
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
    }
} 
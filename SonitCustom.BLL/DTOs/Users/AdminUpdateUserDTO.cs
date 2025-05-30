using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs.Users
{
    public class AdminUpdateUserDTO
    {
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải có từ 3 đến 50 ký tự")]
        public string? Username { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string? Password { get; set; }

        public string? Fullname { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }

        public int? Role { get; set; }
    }
} 
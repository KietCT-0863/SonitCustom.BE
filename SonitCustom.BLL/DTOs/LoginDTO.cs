using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }
    }
}

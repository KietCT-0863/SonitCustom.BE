using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs.Users
{
    /// <summary>
    /// DTO dùng để đăng ký tài khoản người dùng mới
    /// </summary>
    public class RegisterUserDTO
    {
        /// <summary>
        /// Tên đăng nhập của người dùng
        /// </summary>
        /// <remarks>Bắt buộc phải nhập, độ dài từ 3-50 ký tự, không được trùng với tên đăng nhập đã tồn tại</remarks>
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải có từ 3 đến 50 ký tự")]
        public string Username { get; set; }

        /// <summary>
        /// Mật khẩu của người dùng
        /// </summary>
        /// <remarks>Bắt buộc phải nhập, độ dài tối thiểu 6 ký tự</remarks>
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; }

        /// <summary>
        /// Họ tên đầy đủ của người dùng
        /// </summary>
        /// <remarks>Bắt buộc phải nhập</remarks>
        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string Fullname { get; set; }

        /// <summary>
        /// Địa chỉ email của người dùng
        /// </summary>
        /// <remarks>Bắt buộc phải nhập, phải đúng định dạng email, không được trùng với email đã tồn tại</remarks>
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
    }
} 
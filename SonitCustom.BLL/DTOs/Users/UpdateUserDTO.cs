using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs.Users
{
    /// <summary>
    /// DTO dùng để cập nhật thông tin người dùng (do chính người dùng thực hiện)
    /// </summary>
    public class UpdateUserDTO
    {
        /// <summary>
        /// Tên đăng nhập mới (null nếu không cập nhật)
        /// </summary>
        /// <remarks>Độ dài từ 3-50 ký tự, không được trùng với tên đăng nhập đã tồn tại</remarks>
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập phải có từ 3 đến 50 ký tự")]
        public string? Username { get; set; }

        /// <summary>
        /// Mật khẩu mới (null nếu không cập nhật)
        /// </summary>
        /// <remarks>Độ dài tối thiểu 6 ký tự</remarks>
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string? Password { get; set; }

        /// <summary>
        /// Họ tên đầy đủ mới (null nếu không cập nhật)
        /// </summary>
        public string? Fullname { get; set; }

        /// <summary>
        /// Địa chỉ email mới (null nếu không cập nhật)
        /// </summary>
        /// <remarks>Phải đúng định dạng email, không được trùng với email đã tồn tại</remarks>
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string? Email { get; set; }
    }
} 
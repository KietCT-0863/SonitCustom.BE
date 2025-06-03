using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs.Roles
{
    /// <summary>
    /// DTO dùng để tạo mới role người dùng
    /// </summary>
    public class CreateRoleDTO
    {
        /// <summary>
        /// Tên role mới
        /// </summary>
        /// <remarks>Bắt buộc phải có, không được trùng với tên role đã tồn tại</remarks>
        [Required(ErrorMessage = "Tên role không được để trống")]
        public string RoleName { get; set; }
    }
}

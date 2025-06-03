namespace SonitCustom.BLL.DTOs.Roles
{
    /// <summary>
    /// DTO dùng để cập nhật thông tin role người dùng
    /// </summary>
    public class UpdateRoleDTO
    {
        /// <summary>
        /// Tên role mới (null nếu không cập nhật)
        /// </summary>
        /// <remarks>Không được trùng với tên role đã tồn tại</remarks>
        public string? RoleName { get; set; }
    }
}

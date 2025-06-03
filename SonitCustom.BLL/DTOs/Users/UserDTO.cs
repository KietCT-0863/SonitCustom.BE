namespace SonitCustom.BLL.DTOs.Users
{
    /// <summary>
    /// DTO chứa thông tin cơ bản của người dùng
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// ID của người dùng
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// Địa chỉ email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Họ tên đầy đủ
        /// </summary>
        public string Fullname { get; set; }
        
        /// <summary>
        /// Tên role của người dùng
        /// </summary>
        public string RoleName { get; set; }
    }
} 
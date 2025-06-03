namespace SonitCustom.BLL.DTOs.Users
{
    /// <summary>
    /// DTO chứa thông tin chi tiết của người dùng để trả về khi xem thông tin cá nhân
    /// </summary>
    public class RespondUserDTO
    {
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// Họ tên đầy đủ
        /// </summary>
        public string Fullname { get; set; }
        
        /// <summary>
        /// Địa chỉ email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Mật khẩu (đã được mã hóa)
        /// </summary>
        public string Password { get; set; }
        
        /// <summary>
        /// Tên role của người dùng
        /// </summary>
        public string Rolename { get; set; }
    }
}

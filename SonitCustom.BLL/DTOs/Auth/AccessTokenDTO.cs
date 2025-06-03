namespace SonitCustom.BLL.DTOs.Auth
{
    /// <summary>
    /// DTO chứa thông tin về access token
    /// </summary>
    public class AccessTokenDTO
    {
        /// <summary>
        /// Chuỗi token JWT
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// Thời gian hết hạn của token
        /// </summary>
        public DateTime ExpiresAt { get; set; }
        
        /// <summary>
        /// ID của người dùng sở hữu token
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Vai trò của người dùng
        /// </summary>
        public string Role { get; set; }
    }
} 
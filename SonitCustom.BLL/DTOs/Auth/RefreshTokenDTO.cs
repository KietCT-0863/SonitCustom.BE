namespace SonitCustom.BLL.DTOs.Auth
{
    /// <summary>
    /// DTO chứa thông tin về refresh token
    /// </summary>
    public class RefreshTokenDTO
    {
        /// <summary>
        /// Chuỗi refresh token
        /// </summary>
        public string Token { get; set; }
        
        /// <summary>
        /// ID của người dùng sở hữu token
        /// </summary>
        public int UserId { get; set; }
        
        /// <summary>
        /// Thời gian hết hạn của token
        /// </summary>
        public DateTime ExpiresAt { get; set; }
        
        /// <summary>
        /// Thời gian tạo token
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
} 
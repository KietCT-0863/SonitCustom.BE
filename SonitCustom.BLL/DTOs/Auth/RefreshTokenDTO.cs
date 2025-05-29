namespace SonitCustom.BLL.DTOs.Auth
{
    public class RefreshTokenDTO
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 
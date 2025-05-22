namespace SonitCustom.BLL.DTOs
{
    public class RefreshTokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 
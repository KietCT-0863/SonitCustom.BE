namespace SonitCustom.BLL.DTOs.Auth
{
    public class AccessTokenDTO
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int UserId { get; set; }
        public string Role { get; set; }
    }
} 
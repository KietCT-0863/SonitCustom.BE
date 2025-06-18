namespace SonitCustom.BLL.DTOs.Auth
{
    /// <summary>
    /// Đối tượng dữ liệu yêu cầu làm mới access token
    /// </summary>
    public class RefreshTokenRequestDTO
    {
        /// <summary>
        /// Token làm mới
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}
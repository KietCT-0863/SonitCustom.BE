using Microsoft.Extensions.Configuration;

namespace SonitCustom.BLL.Settings
{
    /// <summary>
    /// Cấu hình thiết lập cho hệ thống token xác thực
    /// </summary>
    public class TokenSettings
    {
        /// <summary>
        /// Chuỗi bí mật dùng để ký access token
        /// </summary>
        public string AccessTokenSecret { get; }
        
        /// <summary>
        /// Chuỗi bí mật dùng để ký refresh token
        /// </summary>
        public string RefreshTokenSecret { get; }
        
        /// <summary>
        /// Thời gian hết hạn của access token tính bằng phút
        /// </summary>
        public int AccessTokenExpirationMinutes { get; }
        
        /// <summary>
        /// Thời gian hết hạn của refresh token tính bằng ngày
        /// </summary>
        public int RefreshTokenExpirationDays { get; }
        
        /// <summary>
        /// Khởi tạo cấu hình TokenSettings từ IConfiguration
        /// </summary>
        /// <param name="configuration">Đối tượng cấu hình của ứng dụng</param>
        public TokenSettings(IConfiguration configuration)
        {
            AccessTokenSecret = configuration["JwtSettings:AccessTokenSecret"];
            RefreshTokenSecret = configuration["JwtSettings:RefreshTokenSecret"];
            AccessTokenExpirationMinutes = int.Parse(configuration["JwtSettings:AccessTokenExpirationMinutes"]);
            RefreshTokenExpirationDays = int.Parse(configuration["JwtSettings:RefreshTokenExpirationDays"]);
        }
    }
} 
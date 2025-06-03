using Microsoft.Extensions.Configuration;

namespace SonitCustom.BLL.Settings
{
    /// <summary>
    /// Cấu hình JWT (JSON Web Token) cho xác thực và ủy quyền
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Khóa bí mật dùng để ký JWT
        /// </summary>
        public string Key { get; }
        
        /// <summary>
        /// Tổ chức phát hành JWT
        /// </summary>
        public string Issuer { get; }
        
        /// <summary>
        /// Đối tượng nhận JWT
        /// </summary>
        public string Audience { get; }
        
        /// <summary>
        /// Thời gian hết hạn của JWT tính bằng phút
        /// </summary>
        public double ExpiresInMinutes { get; }

        /// <summary>
        /// Khởi tạo cấu hình JwtSettings từ IConfiguration
        /// </summary>
        /// <param name="configuration">Đối tượng cấu hình của ứng dụng</param>
        public JwtSettings(IConfiguration configuration)
        {
            IConfigurationSection jwtSettings = configuration.GetSection("Jwt");
            Key = jwtSettings["Key"];
            Issuer = jwtSettings["Issuer"];
            Audience = jwtSettings["Audience"];
            ExpiresInMinutes = Convert.ToDouble(jwtSettings["Expires"]);
        }
    }
} 
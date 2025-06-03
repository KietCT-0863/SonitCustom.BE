using Microsoft.Extensions.Configuration;

namespace SonitCustom.BLL.Settings
{
    /// <summary>
    /// Cấu hình kết nối đến Cloudflare R2 Storage
    /// </summary>
    public class R2Settings
    {
        /// <summary>
        /// Tên bucket lưu trữ trên Cloudflare R2
        /// </summary>
        public string BucketName { get; }
        
        /// <summary>
        /// Khóa truy cập (Access Key) cho Cloudflare R2
        /// </summary>
        public string AccessKey { get; }
        
        /// <summary>
        /// Khóa bí mật (Secret Key) cho Cloudflare R2
        /// </summary>
        public string SecretKey { get; }
        
        /// <summary>
        /// ID tài khoản Cloudflare
        /// </summary>
        public string AccountId { get; }
        
        /// <summary>
        /// URL công khai để truy cập tài nguyên trên R2
        /// </summary>
        public string PublicUrl { get; }
        
        /// <summary>
        /// Khởi tạo cấu hình R2Settings từ IConfiguration
        /// </summary>
        /// <param name="configuration">Đối tượng cấu hình của ứng dụng</param>
        public R2Settings(IConfiguration configuration)
        {
            IConfigurationSection r2Settings = configuration.GetSection("Cloudflare:R2");
            BucketName = r2Settings["BucketName"];
            AccessKey = r2Settings["AccessKey"];
            SecretKey = r2Settings["SecretKey"];
            AccountId = r2Settings["AccountId"];
            PublicUrl = r2Settings["PublicUrl"];
        }
    }
} 
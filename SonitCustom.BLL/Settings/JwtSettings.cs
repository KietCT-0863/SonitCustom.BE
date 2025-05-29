using Microsoft.Extensions.Configuration;

namespace SonitCustom.BLL.Settings
{
    public class JwtSettings
    {
        public string Key { get; }
        public string Issuer { get; }
        public string Audience { get; }
        public double ExpiresInMinutes { get; }

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
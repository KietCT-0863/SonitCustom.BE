using Microsoft.Extensions.Configuration;

namespace SonitCustom.BLL.Settings
{
    public class TokenSettings
    {
        public string AccessTokenSecret { get; }
        public string RefreshTokenSecret { get; }
        public int AccessTokenExpirationMinutes { get; }
        public int RefreshTokenExpirationDays { get; }

        public TokenSettings(IConfiguration configuration)
        {
            AccessTokenSecret = configuration["JwtSettings:AccessTokenSecret"];
            RefreshTokenSecret = configuration["JwtSettings:RefreshTokenSecret"];
            AccessTokenExpirationMinutes = int.Parse(configuration["JwtSettings:AccessTokenExpirationMinutes"]);
            RefreshTokenExpirationDays = int.Parse(configuration["JwtSettings:RefreshTokenExpirationDays"]);
        }
    }
} 
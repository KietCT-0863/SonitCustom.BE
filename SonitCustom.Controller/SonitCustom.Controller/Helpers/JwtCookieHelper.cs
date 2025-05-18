using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace SonitCustom.Controller.Helpers
{
    public class JwtCookieHelper
    {
        public static (int? userId, string role)? GetUserInfoFromCookie(HttpRequest request)
        {
            var token = request.Cookies["jwt_token"];
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;
            try
            {
                jwtToken = handler.ReadJwtToken(token);
            }
            catch
            {
                return null;
            }

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userid");
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            if (userIdClaim == null || roleClaim == null)
                return null;

            if (!int.TryParse(userIdClaim.Value, out int userId))
                return null;

            return (userId, roleClaim.Value);
        }

        public static void SetJwtCookie(HttpResponse response, string token)
        {
            response.Cookies.Append("jwt_token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Chỉ gửi qua HTTPS
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(60)
            });
        }

        public static bool IsAdmin(HttpRequest request)
        {
            var token = request.Cookies["jwt_token"];
            if (string.IsNullOrEmpty(token)) return false;

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;
            try
            {
                jwtToken = handler.ReadJwtToken(token);
            }
            catch
            {
                return false;
            }

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            var role = roleClaim?.Value;
            return role != null && role.ToLower() == "admin";
        }
    }
} 
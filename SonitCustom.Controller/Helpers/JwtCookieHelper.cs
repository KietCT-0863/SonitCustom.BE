using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace SonitCustom.Controller.Helpers
{
    public class JwtCookieHelper
    {
        public static (int? userId, string role)? GetUserInfoFromCookie(HttpRequest request)
        {
            string? token = request.Cookies["jwt_token"];

            if (string.IsNullOrEmpty(token))
                return null;

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;

            try
            {
                jwtToken = handler.ReadJwtToken(token);
            }
            catch
            {
                return null;
            }

            Claim? userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userid");
            Claim? roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");

            if (userIdClaim == null || roleClaim == null)
            {
                return null;
            }    

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return null;
            }    

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
            string? token = request.Cookies["jwt_token"];
            if (string.IsNullOrEmpty(token)) return false;

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken;

            try
            {
                jwtToken = handler.ReadJwtToken(token);
            }
            catch
            {
                return false;
            }

            Claim? roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "role");
            string? role = roleClaim?.Value;

            return role != null && role.ToLower() == "admin";
        }
    }
} 
using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface.Security;
using System.Security.Claims;
using Microsoft.Net.Http.Headers;

namespace SonitCustom.Controller.Helpers
{
    /// <summary>
    /// Lớp helper cung cấp các phương thức xử lý JWT token
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// Lấy access token từ header Authorization
        /// </summary>
        /// <param name="request">Request HTTP hiện tại</param>
        /// <returns>Chuỗi access token hoặc null nếu không tồn tại</returns>
        public static string? GetAccessTokenFromHeader(HttpRequest request)
        {
            if (!request.Headers.TryGetValue(HeaderNames.Authorization, out var authHeader))
            {
                return null;
            }

            string auth = authHeader.ToString();
            if (string.IsNullOrEmpty(auth) || !auth.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return auth.Substring("Bearer ".Length).Trim();
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của access token
        /// </summary>
        /// <param name="request">Request HTTP hiện tại</param>
        /// <param name="tokenService">Service xử lý các thao tác với token</param>
        /// <returns>True nếu token hợp lệ, ngược lại False</returns>
        public static bool ValidateAccessToken(HttpRequest request, ITokenService tokenService)
        {
            string? accessToken = GetAccessTokenFromHeader(request);
            if (string.IsNullOrEmpty(accessToken))
            {
                return false;
            }

            return tokenService.ValidateAccessToken(accessToken);
        }

        /// <summary>
        /// Lấy ID người dùng từ token đã xác thực
        /// </summary>
        /// <param name="user">Thông tin người dùng từ token đã xác thực</param>
        /// <returns>ID người dùng</returns>
        /// <exception cref="UnauthorizedAccessException">Ném ra khi không tìm thấy thông tin người dùng trong token</exception>
        public static int GetUserIdFromToken(ClaimsPrincipal user)
        {
            int userId = int.Parse(user.FindFirst("userid")?.Value ?? "0");
            if (userId == 0)
            {
                throw new UnauthorizedAccessException("Token không hợp lệ hoặc không chứa thông tin người dùng");
            }
            
            return userId;
        }
    }
} 
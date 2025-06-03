using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface.Security;
using System.Security.Claims;

namespace SonitCustom.Controller.Helpers
{
    /// <summary>
    /// Lớp helper cung cấp các phương thức xử lý cookie xác thực
    /// </summary>
    public static class CookieHelper
    {
        private const string ACCESS_TOKEN_COOKIE_NAME = "access_token";
        private const string REFRESH_TOKEN_COOKIE_NAME = "refresh_token";

        /// <summary>
        /// Tạo các thiết lập tiêu chuẩn cho cookie
        /// </summary>
        /// <param name="expires">Thời gian hết hạn của cookie (tùy chọn)</param>
        /// <returns>Đối tượng <see cref="CookieOptions"/> với các thiết lập bảo mật</returns>
        private static CookieOptions CreateCookieOptions(DateTime? expires = null)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = expires,
                Path = "/"
            };
        }

        /// <summary>
        /// Thiết lập một cookie xác thực với tên và giá trị cho trước
        /// </summary>
        /// <param name="response">Response HTTP của request hiện tại</param>
        /// <param name="cookieName">Tên cookie</param>
        /// <param name="token">Giá trị token</param>
        /// <param name="expires">Thời gian hết hạn</param>
        private static void SetAuthCookie(HttpResponse response, string cookieName, string token, DateTime expires)
        {
            response.Cookies.Append(cookieName, token, CreateCookieOptions(expires));
        }

        /// <summary>
        /// Lấy giá trị của một cookie xác thực
        /// </summary>
        /// <param name="request">Request HTTP hiện tại</param>
        /// <param name="cookieName">Tên cookie</param>
        /// <returns>Giá trị cookie hoặc null nếu không tồn tại</returns>
        private static string? GetAuthCookie(HttpRequest request, string cookieName)
        {
            return request.Cookies[cookieName];
        }

        /// <summary>
        /// Xóa một cookie xác thực
        /// </summary>
        /// <param name="response">Response HTTP của request hiện tại</param>
        /// <param name="cookieName">Tên cookie cần xóa</param>
        private static void RemoveAuthCookie(HttpResponse response, string cookieName)
        {
            response.Cookies.Delete(cookieName, CreateCookieOptions());
        }

        /// <summary>
        /// Thiết lập cookie cho access token
        /// </summary>
        /// <param name="response">Response HTTP của request hiện tại</param>
        /// <param name="accessToken">Đối tượng <see cref="AccessTokenDTO"/> chứa thông tin access token</param>
        public static void SetAccessTokenCookie(HttpResponse response, AccessTokenDTO accessToken)
        {
            SetAuthCookie(response, ACCESS_TOKEN_COOKIE_NAME, accessToken.Token, accessToken.ExpiresAt);
        }

        /// <summary>
        /// Lấy access token từ cookie
        /// </summary>
        /// <param name="request">Request HTTP hiện tại</param>
        /// <returns>Chuỗi access token hoặc null nếu không tồn tại</returns>
        public static string? GetAccessToken(HttpRequest request)
        {
            return GetAuthCookie(request, ACCESS_TOKEN_COOKIE_NAME);
        }

        /// <summary>
        /// Xóa cookie chứa access token
        /// </summary>
        /// <param name="response">Response HTTP của request hiện tại</param>
        public static void RemoveAccessTokenCookie(HttpResponse response)
        {
            RemoveAuthCookie(response, ACCESS_TOKEN_COOKIE_NAME);
        }

        /// <summary>
        /// Thiết lập cookie cho refresh token
        /// </summary>
        /// <param name="response">Response HTTP của request hiện tại</param>
        /// <param name="refreshToken">Đối tượng <see cref="RefreshTokenDTO"/> chứa thông tin refresh token</param>
        public static void SetRefreshTokenCookie(HttpResponse response, RefreshTokenDTO refreshToken)
        {
            SetAuthCookie(response, REFRESH_TOKEN_COOKIE_NAME, refreshToken.Token, refreshToken.ExpiresAt);
        }

        /// <summary>
        /// Lấy refresh token từ cookie
        /// </summary>
        /// <param name="request">Request HTTP hiện tại</param>
        /// <returns>Chuỗi refresh token hoặc null nếu không tồn tại</returns>
        public static string? GetRefreshToken(HttpRequest request)
        {
            return GetAuthCookie(request, REFRESH_TOKEN_COOKIE_NAME);
        }

        /// <summary>
        /// Xóa cookie chứa refresh token
        /// </summary>
        /// <param name="response">Response HTTP của request hiện tại</param>
        public static void RemoveRefreshTokenCookie(HttpResponse response)
        {
            RemoveAuthCookie(response, REFRESH_TOKEN_COOKIE_NAME);
        }

        /// <summary>
        /// Xóa tất cả cookie xác thực
        /// </summary>
        /// <param name="response">Response HTTP của request hiện tại</param>
        public static void RemoveAllAuthCookies(HttpResponse response)
        {
            RemoveAccessTokenCookie(response);
            RemoveRefreshTokenCookie(response);
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của access token và trả về token nếu hợp lệ
        /// </summary>
        /// <param name="request">Request HTTP hiện tại</param>
        /// <param name="tokenService">Service xử lý các thao tác với token</param>
        /// <returns>Access token hợp lệ</returns>
        /// <exception cref="UnauthorizedAccessException">Ném ra khi không tìm thấy hoặc token không hợp lệ</exception>
        private static string ValidateAndGetAccessToken(HttpRequest request, ITokenService tokenService)
        {
            string? accessToken = GetAccessToken(request);
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new UnauthorizedAccessException("Không tìm thấy access token");
            }

            if (!tokenService.ValidateAccessToken(accessToken))
            {
                throw new UnauthorizedAccessException("Access token không hợp lệ hoặc đã hết hạn");
            }

            return accessToken;
        }

        /// <summary>
        /// Kiểm tra và làm mới access token nếu đã hết hạn
        /// </summary>
        /// <param name="request">Request HTTP hiện tại</param>
        /// <param name="response">Response HTTP của request hiện tại</param>
        /// <param name="tokenService">Service xử lý các thao tác với token</param>
        /// <exception cref="UnauthorizedAccessException">Ném ra khi không thể làm mới access token</exception>
        /// <exception cref="InvalidRefreshTokenException">Ném ra khi refresh token không hợp lệ</exception>
        public static async Task TryRefreshAccessToken(
            HttpRequest request,
            HttpResponse response,
            ITokenService tokenService)
        {
            string? accessToken = GetAccessToken(request);
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new UnauthorizedAccessException("Không tìm thấy access token");
            }

            if (!tokenService.ValidateAccessToken(accessToken))
            {
                await RefreshAccessTokenUsingRefreshToken(request, response, tokenService);
            }
        }

        /// <summary>
        /// Làm mới access token bằng cách sử dụng refresh token
        /// </summary>
        /// <param name="request">Request HTTP hiện tại</param>
        /// <param name="response">Response HTTP của request hiện tại</param>
        /// <param name="tokenService">Service xử lý các thao tác với token</param>
        /// <exception cref="UnauthorizedAccessException">Ném ra khi không thể làm mới access token</exception>
        private static async Task RefreshAccessTokenUsingRefreshToken(
            HttpRequest request,
            HttpResponse response,
            ITokenService tokenService)
        {
            string? refreshToken = GetRefreshToken(request);
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new UnauthorizedAccessException("Không tìm thấy refresh token");
            }

            try
            {
                AccessTokenDTO newAccessToken = await tokenService.RefreshAccessTokenAsync(refreshToken);
                SetAccessTokenCookie(response, newAccessToken);
            }
            catch (InvalidRefreshTokenException)
            {
                throw new UnauthorizedAccessException("Refresh token đã hết hạn, vui lòng đăng nhập lại");
            }
            catch
            {
                throw new UnauthorizedAccessException("Không thể làm mới access token");
            }
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
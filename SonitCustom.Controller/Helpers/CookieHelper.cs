using Microsoft.AspNetCore.Http;
using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using System;
using System.Threading.Tasks;

namespace SonitCustom.Controller.Helpers
{
    public static class CookieHelper
    {
        private const string ACCESS_TOKEN_COOKIE_NAME = "access_token";
        private const string REFRESH_TOKEN_COOKIE_NAME = "refresh_token";

        private static CookieOptions CreateCookieOptions(DateTime? expires = null)
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict, // Nếu dùng nhiều domain, đổi thành SameSiteMode.None
                Expires = expires,
                Path = "/"
            };
        }

        public static void SetAccessTokenCookie(HttpResponse response, AccessTokenDTO accessToken)
        {
            response.Cookies.Append(ACCESS_TOKEN_COOKIE_NAME, accessToken.Token, CreateCookieOptions(accessToken.ExpiresAt));
        }

        public static string? GetAccessToken(HttpRequest request)
        {
            return request.Cookies[ACCESS_TOKEN_COOKIE_NAME];
        }

        public static void RemoveAccessTokenCookie(HttpResponse response)
        {
            response.Cookies.Delete(ACCESS_TOKEN_COOKIE_NAME, CreateCookieOptions());
        }

        public static void SetRefreshTokenCookie(HttpResponse response, RefreshTokenDTO refreshToken)
        {
            response.Cookies.Append(REFRESH_TOKEN_COOKIE_NAME, refreshToken.Token, CreateCookieOptions(refreshToken.ExpiresAt));
        }

        public static string? GetRefreshToken(HttpRequest request)
        {
            return request.Cookies[REFRESH_TOKEN_COOKIE_NAME];
        }

        public static void RemoveRefreshTokenCookie(HttpResponse response)
        {
            response.Cookies.Delete(REFRESH_TOKEN_COOKIE_NAME, CreateCookieOptions());
        }

        public static void RemoveAllAuthCookies(HttpResponse response)
        {
            RemoveAccessTokenCookie(response);
            RemoveRefreshTokenCookie(response);
        }

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
                catch (InvalidRefreshTokenException) // Giả sử bạn định nghĩa exception này trong BLL
                {
                    throw new UnauthorizedAccessException("Refresh token đã hết hạn, vui lòng đăng nhập lại");
                }
                catch
                {
                    throw new UnauthorizedAccessException("Không thể làm mới access token");
                }
            }
        }
    }
}
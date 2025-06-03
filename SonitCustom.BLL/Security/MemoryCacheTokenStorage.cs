using Microsoft.Extensions.Caching.Memory;
using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.Settings;
using System.Collections.Concurrent;
using System.Linq;

namespace SonitCustom.BLL.Security
{
    /// <summary>
    /// Service triển khai lưu trữ refresh token sử dụng bộ nhớ đệm
    /// </summary>
    public class MemoryCacheTokenStorage : ITokenStorage
    {
        private readonly IMemoryCache _cache;
        private readonly TokenSettings _tokenSettings;
        private readonly ConcurrentDictionary<string, int> _tokenToUserIdMap;

        /// <summary>
        /// Khởi tạo đối tượng MemoryCacheTokenStorage
        /// </summary>
        /// <param name="cache">Bộ nhớ đệm</param>
        /// <param name="tokenSettings">Cấu hình token</param>
        public MemoryCacheTokenStorage(IMemoryCache cache, TokenSettings tokenSettings)
        {
            _cache = cache;
            _tokenSettings = tokenSettings;
            _tokenToUserIdMap = new ConcurrentDictionary<string, int>();
        }

        /// <inheritdoc />
        public void StoreRefreshToken(RefreshTokenDTO refreshToken)
        {
            if (_cache.TryGetValue(GetUserCacheKey(refreshToken.UserId), out RefreshTokenDTO oldToken))
            {
                _tokenToUserIdMap.TryRemove(oldToken.Token, out _);
            }

            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(_tokenSettings.RefreshTokenExpirationDays))
                .SetSlidingExpiration(TimeSpan.FromDays(1));

            _cache.Set(GetUserCacheKey(refreshToken.UserId), refreshToken, cacheOptions);
            _tokenToUserIdMap[refreshToken.Token] = refreshToken.UserId;
        }

        /// <inheritdoc />
        public RefreshTokenDTO GetRefreshToken(string token)
        {
            if (_tokenToUserIdMap.TryGetValue(token, out int userId))
            {
                return _cache.Get<RefreshTokenDTO>(GetUserCacheKey(userId));
            }
            return null;
        }

        /// <inheritdoc />
        public RefreshTokenDTO GetRefreshTokenByUserId(int userId)
        {
            return _cache.Get<RefreshTokenDTO>(GetUserCacheKey(userId));
        }

        /// <inheritdoc />
        public void RemoveRefreshToken(string token)
        {
            if (_tokenToUserIdMap.TryGetValue(token, out int userId))
            {
                _cache.Remove(GetUserCacheKey(userId));
                _tokenToUserIdMap.TryRemove(token, out _);
            }
        }

        /// <inheritdoc />
        public bool RefreshTokenExists(string token)
        {
            return _tokenToUserIdMap.ContainsKey(token);
        }

        /// <summary>
        /// Tạo khóa cache cho user ID
        /// </summary>
        /// <param name="userId">ID của người dùng</param>
        /// <returns>Khóa cache dưới dạng chuỗi</returns>
        private string GetUserCacheKey(int userId)
        {
            return $"RefreshToken_User_{userId}";
        }
    }
} 
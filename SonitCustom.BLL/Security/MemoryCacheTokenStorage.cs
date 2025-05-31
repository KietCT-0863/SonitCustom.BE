using Microsoft.Extensions.Caching.Memory;
using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.Settings;
using System.Collections.Concurrent;
using System.Linq;

namespace SonitCustom.BLL.Security
{
    public class MemoryCacheTokenStorage : ITokenStorage
    {
        private readonly IMemoryCache _cache;
        private readonly TokenSettings _tokenSettings;
        private readonly ConcurrentDictionary<string, int> _tokenToUserIdMap;

        public MemoryCacheTokenStorage(IMemoryCache cache, TokenSettings tokenSettings)
        {
            _cache = cache;
            _tokenSettings = tokenSettings;
            _tokenToUserIdMap = new ConcurrentDictionary<string, int>();
        }

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

        public RefreshTokenDTO GetRefreshToken(string token)
        {
            if (_tokenToUserIdMap.TryGetValue(token, out int userId))
            {
                return _cache.Get<RefreshTokenDTO>(GetUserCacheKey(userId));
            }
            return null;
        }

        public RefreshTokenDTO GetRefreshTokenByUserId(int userId)
        {
            return _cache.Get<RefreshTokenDTO>(GetUserCacheKey(userId));
        }

        public void RemoveRefreshToken(string token)
        {
            if (_tokenToUserIdMap.TryGetValue(token, out int userId))
            {
                _cache.Remove(GetUserCacheKey(userId));
                _tokenToUserIdMap.TryRemove(token, out _);
            }
        }

        public bool RefreshTokenExists(string token)
        {
            return _tokenToUserIdMap.ContainsKey(token);
        }

        private string GetUserCacheKey(int userId)
        {
            return $"RefreshToken_User_{userId}";
        }
    }
} 
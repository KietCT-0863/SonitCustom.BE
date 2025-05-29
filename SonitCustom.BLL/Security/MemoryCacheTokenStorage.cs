using Microsoft.Extensions.Caching.Memory;
using SonitCustom.BLL.DTOs.Auth;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.Settings;

namespace SonitCustom.BLL.Security
{
    public class MemoryCacheTokenStorage : ITokenStorage
    {
        private readonly IMemoryCache _cache;
        private readonly TokenSettings _tokenSettings;

        public MemoryCacheTokenStorage(IMemoryCache cache, TokenSettings tokenSettings)
        {
            _cache = cache;
            _tokenSettings = tokenSettings;
        }

        public void StoreRefreshToken(RefreshTokenDTO refreshToken)
        {
            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(_tokenSettings.RefreshTokenExpirationDays))
                .SetSlidingExpiration(TimeSpan.FromDays(1));

            _cache.Set(refreshToken.Token, refreshToken, cacheOptions);
        }

        public RefreshTokenDTO GetRefreshToken(string token)
        {
            return _cache.Get<RefreshTokenDTO>(token);
        }

        public void RemoveRefreshToken(string token)
        {
            _cache.Remove(token);
        }

        public bool RefreshTokenExists(string token)
        {
            return _cache.TryGetValue(token, out _);
        }
    }
} 
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SonitCustom.DAL.Repositories;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        private readonly IUserRepository _userRepository;
        private readonly string _accessTokenSecret;
        private readonly string _refreshTokenSecret;
        private readonly int _accessTokenExpirationMinutes;
        private readonly int _refreshTokenExpirationDays;
        private const string TOKEN_PREFIX = "token_";

        public TokenService(IConfiguration configuration, IMemoryCache cache, IUserRepository userRepository)
        {
            _configuration = configuration;
            _cache = cache;
            _userRepository = userRepository;
            _accessTokenSecret = _configuration["JwtSettings:AccessTokenSecret"];
            _refreshTokenSecret = _configuration["JwtSettings:RefreshTokenSecret"];
            _accessTokenExpirationMinutes = int.Parse(_configuration["JwtSettings:AccessTokenExpirationMinutes"]);
            _refreshTokenExpirationDays = int.Parse(_configuration["JwtSettings:RefreshTokenExpirationDays"]);
        }

        public AccessTokenDTO GenerateAccessToken(int userId, string role)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_accessTokenSecret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("userid", userId.ToString()),
                    new Claim("role", role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return new AccessTokenDTO
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresAt = tokenDescriptor.Expires.Value,
                UserId = userId,
                Role = role
            };
        }

        public RefreshTokenDTO GenerateRefreshToken(int userId)
        {
            byte[] randomNumber = new byte[32];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            string refreshToken = Convert.ToBase64String(randomNumber);

            RefreshTokenDTO refreshTokenDTO = new RefreshTokenDTO
            {
                Token = refreshToken,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow,
            };

            MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromDays(_refreshTokenExpirationDays))
                .SetSlidingExpiration(TimeSpan.FromDays(1));

            _cache.Set(refreshToken, refreshTokenDTO, cacheOptions);

            return refreshTokenDTO;
        }

        public async Task<AccessTokenDTO> RefreshAccessTokenAsync(string refreshToken)
        {
            RefreshTokenDTO? validRefreshToken = await ValidateRefreshTokenAsync(refreshToken);
            if (validRefreshToken == null)
            {
                throw new InvalidRefreshTokenException("Invalid refresh token");
            }

            string? role = await _userRepository.GetRoleByUserIdAsync(validRefreshToken.UserId);
            if (string.IsNullOrEmpty(role))
            {
                throw new Exception("User role not found");
            }

            return GenerateAccessToken(validRefreshToken.UserId, role);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            RefreshTokenDTO? token = await ValidateRefreshTokenAsync(refreshToken);
            if (token != null)
            {
                string cacheKey = $"{TOKEN_PREFIX}{token.UserId}";
                _cache.Remove(cacheKey);
            }
        }

        public async Task<RefreshTokenDTO?> ValidateRefreshTokenAsync(string refreshToken)
        {
            var token = _cache.Get<RefreshTokenDTO>(refreshToken);
            if (token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                return null;
            }

            return token;
        }

        public bool ValidateAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_accessTokenSecret);

                tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out _);

                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                return false;
            }
        }

    }
} 
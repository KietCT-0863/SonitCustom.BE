using System.Text;
using SonitCustom.BLL.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.DTOs.Users;

namespace SonitCustom.BLL.Security
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IConfiguration configuration)
        {
            _jwtSettings = new JwtSettings(configuration);
        }

        public Task<string> GenerateTokenAsync(UserDTO userDto)
        {
            Claim[] claims = CreateClaims(userDto);
            SigningCredentials credentials = CreateSigningCredentials();
            JwtSecurityToken token = CreateJwtToken(claims, credentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(tokenString);
        }

        private Claim[] CreateClaims(UserDTO userDto)
        {
            return new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userDto.Username),
                new Claim("userid", userDto.Id.ToString()),
                new Claim("role", userDto.RoleName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
        }

        private SigningCredentials CreateSigningCredentials()
        {
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials credentials)
        {
            return new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpiresInMinutes),
                signingCredentials: credentials
            );
        }
    }
}
using System.Text;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SonitCustom.DAL.Entities;
using SonitCustom.BLL.DTOs;

namespace SonitCustom.BLL.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public LoginService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<UserDTO?> LoginAsync(string username, string password)
        {
            User user = await _userRepository.GetUserAsync(username, password);

            if (user == null)
            {
                return null;
            }    

            UserDTO userDTO = new()
            {
                Id = user.id,
                Username = user.username,
                Email = user.email,
                Fullname = user.fullname,
                RoleName = user.roleNavigation?.roleName
            };
            
            return userDTO;
        }

        public Task<string> GenerateJwtTokenAsync(UserDTO userDto)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("Jwt");

            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userDto.Username),
                new Claim("userid", userDto.Id.ToString()),
                new Claim("role", userDto.RoleName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["Expires"])),
                signingCredentials: creds
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Task.FromResult(tokenString);
        }
    }
}
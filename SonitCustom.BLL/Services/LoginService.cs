using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<UserDTO> LoginAsync(string username, string password)
        {
            // Gọi repository để lấy user
            user user = await _userRepository.GetUserAsync(username, password);

            // Trả về null nếu không tìm thấy người dùng hoặc thông tin đăng nhập không đúng
            if (user == null)
                return null;

            // Trả về thông tin người dùng nếu đăng nhập thành công
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
            var jwtSettings = _configuration.GetSection("Jwt");
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userDto.Username),
                new Claim("userid", userDto.Id.ToString()),
                new Claim("role", userDto.RoleName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["Expires"])),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(tokenString);
        }
    }
}
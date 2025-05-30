using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Repositories;
using SonitCustom.DAL.Entities;
using SonitCustom.BLL.Interface.Security;
using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Exceptions;

namespace SonitCustom.BLL.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LoginService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<UserDTO> LoginAsync(string username, string password)
        {
            User? user = await GetValidUserAsync(username, password);

            if (user == null)
            {
                throw new InvalidCredentialsException(username);
            }

            return MapUserToDto(user);
        }

        public async Task<string> GenerateJwtTokenAsync(UserDTO userDto)
        {
            return await _jwtService.GenerateTokenAsync(userDto);
        }

        private async Task<User?> GetValidUserAsync(string username, string password)
        {
            return await _userRepository.GetUserAsync(username, password);
        }

        private UserDTO MapUserToDto(User user)
        {
            return new UserDTO
            {
                Id = user.id,
                Username = user.username,
                Email = user.email,
                Fullname = user.fullname,
                RoleName = user.roleNavigation.roleName
            };
        }
    }
}
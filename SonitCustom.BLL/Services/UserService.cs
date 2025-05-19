using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Repositories;

namespace SonitCustom.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = (await _userRepository.GetAllUserAsync()).FirstOrDefault(u => u.id == id);
            if (user == null) return null;
            return new UserDTO
            {
                Id = user.id,
                Username = user.username,
                Fullname = user.fullname,
                Email = user.email,
                RoleName = user.roleNavigation?.roleName
            };
        }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUserAsync();
            return users.Select(user => new UserDTO
            {
                Id = user.id,
                Username = user.username,
                Fullname = user.fullname,
                Email = user.email,
                RoleName = user.roleNavigation?.roleName
            }).ToList();
        }
    }
} 
using System.Threading.Tasks;
using SonitCustom.BLL.DTOs;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Repositories;

namespace SonitCustom.BLL.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;

        public RegisterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<user> RegisterAsync(RegisterUserDTO newRegister)
        {
            user newUser = new user()
            {
                username = newRegister.Username,
                password = newRegister.Password,
                fullname = newRegister.Fullname,
                email = newRegister.Email,
                role = 2
            };

            // Có thể kiểm tra trùng username/email ở đây nếu muốn
            return await _userRepository.AddNewUserAsync(newUser);
        }
    }
} 
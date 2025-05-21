using System;
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

        public async Task<bool> RegisterAsync(RegisterUserDTO newRegister)
        {
            try
            {
                User newUser = new User()
                {
                    username = newRegister.Username,
                    password = newRegister.Password,
                    fullname = newRegister.Fullname,
                    email = newRegister.Email,
                    role = 2
                };

                await _userRepository.AddNewUserAsync(newUser);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi đăng ký người dùng", ex);
            }
        }
    }
}
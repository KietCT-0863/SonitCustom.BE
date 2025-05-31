using SonitCustom.BLL.DTOs.Users;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;

namespace SonitCustom.BLL.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IUserRepository _userRepository;
        private const int DEFAULT_USER_ROLE = 2;

        public RegisterService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> RegisterAsync(RegisterUserDTO newRegister)
        {
            await CheckUserExistenceAsync(newRegister.Username, newRegister.Email);

            User newUser = MapDtoToUserEntity(newRegister);
            
            await _userRepository.AddNewUserAsync(newUser);
            return true;
        }

        private async Task CheckUserExistenceAsync(string username, string email)
        {
            bool userExists = await _userRepository.CheckUserExistsAsync(username, email);
            
            if (userExists)
            {
                throw new UserCredentialsAlreadyExistsException(username, email);
            }
        }

        private User MapDtoToUserEntity(RegisterUserDTO dto)
        {
            return new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Fullname = dto.Fullname,
                Email = dto.Email,
                Role = DEFAULT_USER_ROLE
            };
        }
    }
}
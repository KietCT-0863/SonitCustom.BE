using SonitCustom.BLL.DTOs.Users;

namespace SonitCustom.BLL.Interface
{
    public interface IRegisterService
    {
        Task<bool> RegisterAsync(RegisterUserDTO newUser);
    }
}
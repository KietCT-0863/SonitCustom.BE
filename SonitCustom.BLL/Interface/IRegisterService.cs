using SonitCustom.BLL.DTOs;

namespace SonitCustom.BLL.Interface
{
    public interface IRegisterService
    {
        Task<bool> RegisterAsync(RegisterUserDTO newUser);
    }
}
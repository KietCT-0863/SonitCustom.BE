using SonitCustom.BLL.DTOs.Users;

namespace SonitCustom.BLL.Interface
{
    public interface ILoginService
    {
        Task<UserDTO> LoginAsync(string username, string password);
        Task<string> GenerateJwtTokenAsync(UserDTO userDto);
    }
} 
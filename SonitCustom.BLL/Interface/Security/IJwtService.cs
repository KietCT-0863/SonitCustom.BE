using SonitCustom.BLL.DTOs.Users;

namespace SonitCustom.BLL.Interface.Security
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(UserDTO userDto);
    }
}
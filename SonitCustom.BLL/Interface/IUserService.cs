using SonitCustom.BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SonitCustom.BLL.Interface
{
    public interface IUserService
    {
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<List<UserDTO>> GetAllUsersAsync();
    }
} 
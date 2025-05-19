using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonitCustom.BLL.DTOs;

namespace SonitCustom.BLL.Interface
{
    public interface ILoginService
    {
        Task<UserDTO> LoginAsync(string username, string password);
        Task<string> GenerateJwtTokenAsync(UserDTO user);
    }
} 
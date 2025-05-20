using System.Threading.Tasks;
using SonitCustom.BLL.DTOs;
using SonitCustom.DAL.Entities;

namespace SonitCustom.BLL.Interface
{
    public interface IRegisterService
    {
        Task<User> RegisterAsync(RegisterUserDTO newUser);
    }
}
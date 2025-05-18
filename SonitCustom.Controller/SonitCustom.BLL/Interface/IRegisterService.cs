using System.Threading.Tasks;
using SonitCustom.BLL.DTOs;
using SonitCustom.DAL.Entities;

namespace SonitCustom.BLL.Interface
{
    public interface IRegisterService
    {
        Task<user> RegisterAsync(RegisterUserDTO newUser);
    }
}
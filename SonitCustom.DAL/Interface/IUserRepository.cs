using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<List<user>> GetAllUserAsync();
        Task<user> AddNewUserAsync(user newUser);
        Task<bool> UpdateUserAsync(user userToUpdate);

        Task<user> GetUserAsync(string username, string password);
    }
}
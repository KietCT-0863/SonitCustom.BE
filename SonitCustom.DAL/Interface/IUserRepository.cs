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
        Task<List<User>> GetAllUserAsync();
        Task AddNewUserAsync(User newUser);
        Task UpdateUserAsync(User userToUpdate);

        Task<User> GetUserAsync(string username, string password);
    }
}
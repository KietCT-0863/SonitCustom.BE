using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonitCustom.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace SonitCustom.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SonitCustomDBContext _context;

        public UserRepository(SonitCustomDBContext context)
        {
            _context = context;
        }

        public async Task<List<user>> GetAllUserAsync()
        {
            return await _context.users.Include(u => u.roleNavigation).ToListAsync();
        }

        public async Task<user> AddNewUserAsync(user newUser)
        {
            await _context.users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public async Task<bool> UpdateUserAsync(user userToUpdate)
        {
            var existingUser = await _context.users.FindAsync(userToUpdate.id);
            if (existingUser == null)
                return false;

            _context.Entry(existingUser).CurrentValues.SetValues(userToUpdate);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<user?> GetUserAsync(string username, string password)
        {
            return await _context.users
                .Include(u => u.roleNavigation)
                .FirstOrDefaultAsync(u => u.username == username && u.password == password);
        }
    }
}

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

        public async Task<List<User>> GetAllUserAsync()
        {
            return await _context.Users.Include(u => u.roleNavigation).ToListAsync();
        }

        public async Task AddNewUserAsync(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User userToUpdate)
        {
            _context.Entry(userToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            return await _context.Users
                .Include(u => u.roleNavigation)
                .FirstOrDefaultAsync(u => u.username == username && u.password == password);
        }
    }
}

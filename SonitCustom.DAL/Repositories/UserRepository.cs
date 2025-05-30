using SonitCustom.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using SonitCustom.DAL.Interface;

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

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.roleNavigation)
                .FirstOrDefaultAsync(u => u.id == userId);
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

        public async Task<string?> GetRoleByUserIdAsync(int userId)
        {
            User? user = await _context.Users
                .Include(u => u.roleNavigation)
                .FirstOrDefaultAsync(u => u.id == userId);
            
            return user?.roleNavigation.roleName;
        }

        public async Task<bool> CheckUserExistsAsync(string username, string email)
        {
            return await _context.Users
                .AnyAsync(u => u.username.ToLower() == username.ToLower() || 
                              u.email.ToLower() == email.ToLower());
        }

        public async Task<bool> IsUserNameExistsAsync(string username, int excludeUserId = 0)
        {
            return await _context.Users
                .AnyAsync(u => u.username.ToLower() == username.ToLower() && u.id != excludeUserId);
        }

        public async Task<bool> IsEmailExistsAsync(string email, int excludeUserId = 0)
        {
            return await _context.Users
                .AnyAsync(u => u.email.ToLower() == email.ToLower() && u.id != excludeUserId);
        }
    }
}

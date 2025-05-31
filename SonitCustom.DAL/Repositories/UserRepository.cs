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
            return await _context.Users.Include(u => u.RoleNavigation).ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(u => u.Id == userId);
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
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task<string?> GetRoleByUserIdAsync(int userId)
        {
            User? user = await _context.Users
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            return user?.RoleNavigation.RoleName;
        }

        public async Task<bool> CheckUserExistsAsync(string username, string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Username.ToLower() == username.ToLower() || 
                              u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> IsUserNameExistsAsync(string username, int excludeUserId = 0)
        {
            return await _context.Users
                .AnyAsync(u => u.Username.ToLower() == username.ToLower() && u.Id != excludeUserId);
        }

        public async Task<bool> IsEmailExistsAsync(string email, int excludeUserId = 0)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != excludeUserId);
        }

        public async Task DeleteUserAsync(User userToDelete)
        {
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }
    }
}

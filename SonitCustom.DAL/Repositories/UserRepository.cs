using SonitCustom.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using SonitCustom.DAL.Interface;

namespace SonitCustom.DAL.Repositories
{
    /// <summary>
    /// Cài đặt các thao tác với User trong cơ sở dữ liệu
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly SonitCustomDBContext _context;

        /// <summary>
        /// Khởi tạo đối tượng UserRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public UserRepository(SonitCustomDBContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<List<User>> GetAllUserAsync()
        {
            return await _context.Users.Include(u => u.RoleNavigation).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        /// <inheritdoc />
        public async Task AddNewUserAsync(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateUserAsync(User userToUpdate)
        {
            _context.Entry(userToUpdate).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<User?> GetUserAsync(string username, string password)
        {
            return await _context.Users
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        /// <inheritdoc />
        public async Task<string?> GetRoleByUserIdAsync(int userId)
        {
            User? user = await _context.Users
                .Include(u => u.RoleNavigation)
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            return user?.RoleNavigation.RoleName;
        }

        /// <inheritdoc />
        public async Task<bool> CheckUserExistsAsync(string username, string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Username.ToLower() == username.ToLower() || 
                              u.Email.ToLower() == email.ToLower());
        }

        /// <inheritdoc />
        public async Task<bool> IsUserNameExistsAsync(string username, int excludeUserId = 0)
        {
            return await _context.Users
                .AnyAsync(u => u.Username.ToLower() == username.ToLower() && u.Id != excludeUserId);
        }

        /// <inheritdoc />
        public async Task<bool> IsEmailExistsAsync(string email, int excludeUserId = 0)
        {
            return await _context.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.Id != excludeUserId);
        }

        /// <inheritdoc />
        public async Task DeleteUserAsync(User userToDelete)
        {
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<int> CountUsersByRoleIdAsync(int roleId)
        {
            return await _context.Users.CountAsync(u => u.Role == roleId);
        }
    }
}

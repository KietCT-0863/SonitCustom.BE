using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace SonitCustom.DAL.Repositories
{
    /// <summary>
    /// Cài đặt các thao tác với Role trong cơ sở dữ liệu
    /// </summary>
    public class RoleRepository : IRoleRepository
    {
        private readonly SonitCustomDBContext _context;

        /// <summary>
        /// Khởi tạo đối tượng RoleRepository
        /// </summary>
        /// <param name="context">Database context</param>
        public RoleRepository(SonitCustomDBContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<bool> IsRoleExistsAsync(int roleId)
        {
            return await _context.Roles.AnyAsync(r => r.RoleId == roleId);
        }

        /// <inheritdoc />
        public async Task<bool> IsRoleNameExistsAsync(string roleName)
        {
            return await _context.Roles.AnyAsync(r => r.RoleName == roleName);
        }

        /// <inheritdoc />
        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }
        
        /// <inheritdoc />
        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        /// <inheritdoc />
        public async Task CreateRoleAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }
        
        /// <inheritdoc />
        public async Task UpdateRoleAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }
        
        /// <inheritdoc />
        public async Task DeleteRoleAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
} 
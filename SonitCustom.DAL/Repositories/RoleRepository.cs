using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;
using Microsoft.EntityFrameworkCore;

namespace SonitCustom.DAL.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly SonitCustomDBContext _context;

        public RoleRepository(SonitCustomDBContext context)
        {
            _context = context;
        }

        public async Task<bool> IsRoleExistsAsync(int roleId)
        {
            return await _context.Roles.AnyAsync(r => r.RoleId == roleId);
        }

        public async Task<bool> IsRoleNameExistsAsync(string roleName)
        {
            return await _context.Roles.AnyAsync(r => r.RoleName == roleName);
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }
        
        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        public async Task CreateRoleAsync(Role role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }
        
        public async Task UpdateRoleAsync(Role role)
        {
            _context.Roles.Update(role);
            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteRoleAsync(Role role)
        {
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
} 
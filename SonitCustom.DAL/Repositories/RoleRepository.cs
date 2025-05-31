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

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetRoleByIdAsync(int roleId)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
        }

        public async Task<string?> GetRoleNameByIdAsync(int roleId)
        {
            Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == roleId);
            return role?.RoleName;
        }
    }
} 
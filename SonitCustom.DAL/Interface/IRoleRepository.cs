using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Interface
{
    public interface IRoleRepository
    {
        Task<bool> IsRoleExistsAsync(int roleId);
        Task<bool> IsRoleNameExistsAsync(string roleName);
        Task<List<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task CreateRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(Role role);
    }
}
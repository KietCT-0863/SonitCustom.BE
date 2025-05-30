using SonitCustom.DAL.Entities;

namespace SonitCustom.DAL.Interface
{
    public interface IRoleRepository
    {
        Task<bool> IsRoleExistsAsync(int roleId);
        Task<List<Role>> GetAllRolesAsync();
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<string?> GetRoleNameByIdAsync(int roleId);
    }
} 
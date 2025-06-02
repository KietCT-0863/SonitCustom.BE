using SonitCustom.BLL.DTOs.Roles;

namespace SonitCustom.BLL.Interface
{
    public interface IRoleService
    {
        Task<List<RoleDTO>> GetAllRolesAsync();
        Task<RoleDTO> GetRoleByIdAsync(int roleId);
        Task CreateRoleAsync(CreateRoleDTO createRole);
        Task UpdateRoleAsync(int roleId, UpdateRoleDTO updateRole);
        Task DeleteRoleAsync(int roleId);
    }
}
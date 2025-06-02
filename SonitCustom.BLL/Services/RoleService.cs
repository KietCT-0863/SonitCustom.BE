using SonitCustom.BLL.DTOs.Roles;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;

namespace SonitCustom.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        public async Task<List<RoleDTO>> GetAllRolesAsync()
        {
            List<Role> roles = await _roleRepository.GetAllRolesAsync();
            return roles.Select(r => MapRoleToDTO(r)).ToList();
        }

        public async Task<RoleDTO> GetRoleByIdAsync(int roleId)
        {
            Role? existRole = await GetRoleEntityByIdAsync(roleId);
            return MapRoleToDTO(existRole);
        }

        public async Task CreateRoleAsync(CreateRoleDTO createRole)
        {
            await ValidateRoleNameAsync(createRole.RoleName);

            Role newRole = MapDtoToRoleEntity(createRole);
            await _roleRepository.CreateRoleAsync(newRole);
        }

        public async Task UpdateRoleAsync(int roleId, UpdateRoleDTO updateRole)
        {
            Role existRole = await GetRoleEntityByIdAsync(roleId);

            await ValidateRoleNameAsync(updateRole.RoleName);

            UpdateRoleFields(existRole, updateRole);
            await _roleRepository.UpdateRoleAsync(existRole);
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            Role existRole = await GetRoleEntityByIdAsync(roleId);
            
            await ValidateRoleNotInUseAsync(roleId);
            
            await _roleRepository.DeleteRoleAsync(existRole);
        }

        private async Task<Role> GetRoleEntityByIdAsync(int roleId)
        {
            Role? existRole = await _roleRepository.GetRoleByIdAsync(roleId);
            await ValidateRoleExistsAsync(roleId);
            return existRole;
        }

        private RoleDTO MapRoleToDTO(Role role)
        {
            return new RoleDTO
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }

        private Role MapDtoToRoleEntity(CreateRoleDTO createRoleDto)
        {
            return new Role
            {
                RoleName = createRoleDto.RoleName.ToLower()
            };
        }

        private void UpdateRoleFields(Role existingRole, UpdateRoleDTO updateRoleDto)
        {
            existingRole.RoleName = string.IsNullOrEmpty(updateRoleDto.RoleName) ? existingRole.RoleName.ToLower() : updateRoleDto.RoleName.ToLower();
        }

        private async Task ValidateRoleNameAsync(string roleName)
        {
            if (await _roleRepository.IsRoleNameExistsAsync(roleName))
            {
                throw new RoleNameAlreadyExistsException(roleName);
            }
        }

        private async Task ValidateRoleExistsAsync(int roleId)
        {
            if (!await _roleRepository.IsRoleExistsAsync(roleId))
            {
                throw new RoleNotFoundException(roleId);
            }
        }
        
        private async Task ValidateRoleNotInUseAsync(int roleId)
        {
            int userCount = await _userRepository.CountUsersByRoleIdAsync(roleId);
            if (userCount > 0)
            {
                throw new RoleHasUsersException(roleId, userCount);
            }
        }
    }
}
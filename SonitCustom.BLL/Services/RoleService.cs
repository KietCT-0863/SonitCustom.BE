using SonitCustom.BLL.DTOs.Roles;
using SonitCustom.BLL.Exceptions;
using SonitCustom.BLL.Interface;
using SonitCustom.DAL.Entities;
using SonitCustom.DAL.Interface;

namespace SonitCustom.BLL.Services
{
    /// <summary>
    /// Service triển khai các thao tác liên quan đến role người dùng
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Khởi tạo đối tượng RoleService
        /// </summary>
        /// <param name="roleRepository">Repository truy vấn dữ liệu role</param>
        /// <param name="userRepository">Repository truy vấn dữ liệu user</param>
        public RoleService(IRoleRepository roleRepository, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<List<RoleDTO>> GetAllRolesAsync()
        {
            List<Role> roles = await _roleRepository.GetAllRolesAsync();
            return roles.Select(r => MapRoleToDTO(r)).ToList();
        }

        /// <inheritdoc />
        public async Task<RoleDTO> GetRoleByIdAsync(int roleId)
        {
            Role? existRole = await GetRoleEntityByIdAsync(roleId);
            return MapRoleToDTO(existRole);
        }

        /// <inheritdoc />
        public async Task CreateRoleAsync(CreateRoleDTO createRole)
        {
            await ValidateRoleNameAsync(createRole.RoleName);

            Role newRole = MapDtoToRoleEntity(createRole);
            await _roleRepository.CreateRoleAsync(newRole);
        }

        /// <inheritdoc />
        public async Task UpdateRoleAsync(int roleId, UpdateRoleDTO updateRole)
        {
            Role existRole = await GetRoleEntityByIdAsync(roleId);

            await ValidateRoleNameAsync(updateRole.RoleName);

            UpdateRoleFields(existRole, updateRole);
            await _roleRepository.UpdateRoleAsync(existRole);
        }

        /// <inheritdoc />
        public async Task DeleteRoleAsync(int roleId)
        {
            Role existRole = await GetRoleEntityByIdAsync(roleId);
            
            await ValidateRoleNotInUseAsync(roleId);
            
            await _roleRepository.DeleteRoleAsync(existRole);
        }

        /// <summary>
        /// Lấy đối tượng Role theo ID và kiểm tra tồn tại
        /// </summary>
        /// <param name="roleId">ID của role cần lấy</param>
        /// <returns>Đối tượng Role nếu tồn tại</returns>
        /// <exception cref="RoleNotFoundException">Ném ra khi không tìm thấy role</exception>
        private async Task<Role> GetRoleEntityByIdAsync(int roleId)
        {
            Role? existRole = await _roleRepository.GetRoleByIdAsync(roleId);
            await ValidateRoleExistsAsync(roleId);
            return existRole;
        }

        /// <summary>
        /// Chuyển đổi đối tượng Role thành RoleDTO
        /// </summary>
        /// <param name="role">Đối tượng Role cần chuyển đổi</param>
        /// <returns>Đối tượng RoleDTO</returns>
        private RoleDTO MapRoleToDTO(Role role)
        {
            return new RoleDTO
            {
                RoleId = role.RoleId,
                RoleName = role.RoleName
            };
        }

        /// <summary>
        /// Chuyển đổi đối tượng CreateRoleDTO thành Role
        /// </summary>
        /// <param name="createRoleDto">Đối tượng CreateRoleDTO cần chuyển đổi</param>
        /// <returns>Đối tượng Role</returns>
        private Role MapDtoToRoleEntity(CreateRoleDTO createRoleDto)
        {
            return new Role
            {
                RoleName = createRoleDto.RoleName.ToLower()
            };
        }

        /// <summary>
        /// Cập nhật thông tin role từ UpdateRoleDTO
        /// </summary>
        /// <param name="existingRole">Đối tượng Role cần cập nhật</param>
        /// <param name="updateRoleDto">Dữ liệu cập nhật</param>
        private void UpdateRoleFields(Role existingRole, UpdateRoleDTO updateRoleDto)
        {
            existingRole.RoleName = string.IsNullOrEmpty(updateRoleDto.RoleName) ? existingRole.RoleName.ToLower() : updateRoleDto.RoleName.ToLower();
        }

        /// <summary>
        /// Xác thực tính duy nhất của tên role
        /// </summary>
        /// <param name="roleName">Tên role cần xác thực</param>
        /// <exception cref="RoleNameAlreadyExistsException">Ném ra khi tên role đã tồn tại</exception>
        private async Task ValidateRoleNameAsync(string roleName)
        {
            if (await _roleRepository.IsRoleNameExistsAsync(roleName))
            {
                throw new RoleNameAlreadyExistsException(roleName);
            }
        }

        /// <summary>
        /// Xác thực sự tồn tại của role
        /// </summary>
        /// <param name="roleId">ID của role cần xác thực</param>
        /// <exception cref="RoleNotFoundException">Ném ra khi không tìm thấy role</exception>
        private async Task ValidateRoleExistsAsync(int roleId)
        {
            if (!await _roleRepository.IsRoleExistsAsync(roleId))
            {
                throw new RoleNotFoundException(roleId);
            }
        }
        
        /// <summary>
        /// Kiểm tra role không được gán cho user nào trước khi xóa
        /// </summary>
        /// <param name="roleId">ID của role cần kiểm tra</param>
        /// <exception cref="RoleHasUsersException">Ném ra khi role vẫn còn được gán cho user</exception>
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
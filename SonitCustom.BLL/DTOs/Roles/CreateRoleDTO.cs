using System.ComponentModel.DataAnnotations;

namespace SonitCustom.BLL.DTOs.Roles
{
    public class CreateRoleDTO
    {
        [Required(ErrorMessage = "Tên role không được để trống")]
        public string RoleName { get; set; }
    }
}

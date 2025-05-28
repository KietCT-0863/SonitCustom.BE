namespace SonitCustom.DAL.Entities;

public partial class Role
{
    public int roleId { get; set; }

    public string roleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
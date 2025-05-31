namespace SonitCustom.DAL.Entities;

public partial class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Fullname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    public virtual Role RoleNavigation { get; set; }
}
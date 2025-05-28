namespace SonitCustom.DAL.Entities;

public partial class User
{
    public int id { get; set; }

    public string username { get; set; }

    public string fullname { get; set; }

    public string email { get; set; }

    public string password { get; set; }

    public int role { get; set; }

    public virtual Role roleNavigation { get; set; }
}
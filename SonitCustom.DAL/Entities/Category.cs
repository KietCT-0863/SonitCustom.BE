namespace SonitCustom.DAL.Entities;

public partial class Category
{
    public int CateId { get; set; }

    public string CateName { get; set; }

    public string Prefix { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
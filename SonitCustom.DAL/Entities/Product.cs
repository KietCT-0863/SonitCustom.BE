namespace SonitCustom.DAL.Entities;

public partial class Product
{
    public int Id { get; set; }
    public string ProdId { get; set; }
    public string ProName { get; set; }
    public string Description { get; set; }
    public string ImgUrl { get; set; }
    public double Price { get; set; }
    public int Category { get; set; }
    public bool IsCustom { get; set; }
    public virtual Category CategoryNavigation { get; set; }
}
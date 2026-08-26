namespace Kosa.ECommerce.Persistence.Entities;

public class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();

    public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}

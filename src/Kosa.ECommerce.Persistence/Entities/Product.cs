namespace Kosa.ECommerce.Persistence.Entities;

public class Product
{
    public int ProductId { get; set; }

    public int CategoryId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public decimal Price { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public Category Category { get; set; } = null!;

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
}

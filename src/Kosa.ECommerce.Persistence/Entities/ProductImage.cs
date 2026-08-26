namespace Kosa.ECommerce.Persistence.Entities;

public class ProductImage
{
    public int ImageId { get; set; }

    public int ProductId { get; set; }

    public string ImageUrl { get; set; } = null!;

    public Product Product { get; set; } = null!;
}

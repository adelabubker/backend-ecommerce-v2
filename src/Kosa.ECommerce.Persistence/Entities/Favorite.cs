namespace Kosa.ECommerce.Persistence.Entities;

public class Favorite
{
    public int FavoriteId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public DateTime CreatedDate { get; set; }

    public Product Product { get; set; } = null!;

    public User User { get; set; } = null!;
}

namespace Kosa.ECommerce.Persistence.Entities;

public class Cart
{
    public int CartId { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedDate { get; set; }

    /// <summary>True once the cart has been converted into an order (checked out).</summary>
    public bool IsOrdered { get; set; }

    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public User User { get; set; } = null!;
}

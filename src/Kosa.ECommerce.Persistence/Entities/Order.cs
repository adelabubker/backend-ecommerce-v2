namespace Kosa.ECommerce.Persistence.Entities;

public class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public int AddressId { get; set; }

    public string OrderNumber { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public decimal SubTotal { get; set; }

    public decimal? DeliveryFee { get; set; }

    public decimal? PackagingFee { get; set; }

    public decimal? TotalAmount { get; set; }

    public string OrderStatus { get; set; } = null!;

    public UserAddress Address { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public User User { get; set; } = null!;
}

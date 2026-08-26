namespace Kosa.ECommerce.Persistence.Entities;

public class UserAddress
{
    public int AddressId { get; set; }

    public int UserId { get; set; }

    public string AddressType { get; set; } = null!;

    public string? AddressLine { get; set; }

    public string City { get; set; } = null!;

    public string? State { get; set; }

    public string? PostalCode { get; set; }

    public string Country { get; set; } = null!;

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public bool IsDefault { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public User User { get; set; } = null!;
}

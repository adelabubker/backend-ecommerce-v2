namespace Kosa.ECommerce.Persistence.Entities;

public class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? ProfileImage { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();

    public ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
}

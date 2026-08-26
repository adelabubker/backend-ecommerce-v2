namespace Kosa.ECommerce.Application.DTOs.Favorites;

public sealed class FavoriteDto
{
    public int FavoriteId { get; init; }

    public int UserId { get; init; }

    public int ProductId { get; init; }

    public string ProductName { get; init; } = string.Empty;

    public decimal ProductPrice { get; init; }

    public string? ProductImageUrl { get; init; }

    public DateTime CreatedDate { get; init; }
}

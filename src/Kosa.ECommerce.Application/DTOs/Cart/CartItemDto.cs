namespace Kosa.ECommerce.Application.DTOs.Cart;

public sealed class CartItemDto
{
    public int CartItemId { get; init; }

    public int ProductId { get; init; }

    public string ProductName { get; init; } = string.Empty;

    public string? ImageUrl { get; init; }

    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal LineTotal => UnitPrice * Quantity;
}

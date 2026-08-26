namespace Kosa.ECommerce.Application.DTOs.Cart;

public sealed class CartDto
{
    public int CartId { get; init; }

    public int UserId { get; init; }

    public IReadOnlyList<CartItemDto> Items { get; init; } = [];

    public decimal Total => Items.Sum(item => item.LineTotal);
}

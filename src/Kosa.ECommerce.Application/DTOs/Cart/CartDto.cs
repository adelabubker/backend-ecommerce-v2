
namespace Kosa.ECommerce.Application.DTOs.Cart;

public sealed class CartDto
{
    public int CartId { get; init; }

    public int UserId { get; init; }

    public IReadOnlyList<CartItemDto> Items { get; init; } = [];

    public decimal SubTotal => Items.Sum(item => item.LineTotal);

    public decimal DeliveryFee { get; init; }

    public decimal PackagingFee { get; init; }

    public decimal Total => SubTotal + DeliveryFee + PackagingFee;
}


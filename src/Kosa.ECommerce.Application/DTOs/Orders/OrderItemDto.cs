namespace Kosa.ECommerce.Application.DTOs.Orders;

public sealed class OrderItemDto
{
    public int ProductId { get; init; }

    public string ProductName { get; init; } = string.Empty;

    public int Quantity { get; init; }

    public decimal UnitPrice { get; init; }

    public decimal LineTotal => UnitPrice * Quantity;
}

namespace Kosa.ECommerce.Application.DTOs.Orders;

public sealed class OrderDto
{
    public int OrderId { get; init; }

    public string OrderNumber { get; init; } = string.Empty;

    public DateTime CreatedAt { get; init; }

    public DateTime EstimatedDeliveryAt { get; init; }

    public string Status { get; init; } = string.Empty;

    public decimal SubTotal { get; init; }

    public decimal DeliveryFee { get; init; }

    public decimal PackagingFee { get; init; }

    public decimal Total { get; init; }

    public IReadOnlyList<OrderItemDto> Items { get; init; } = [];
}

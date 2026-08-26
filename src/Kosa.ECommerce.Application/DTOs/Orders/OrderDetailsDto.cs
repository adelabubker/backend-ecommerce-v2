using Kosa.ECommerce.Application.DTOs.Users;

namespace Kosa.ECommerce.Application.DTOs.Orders;

public sealed class OrderDetailsDto
{
    public int OrderId { get; init; }

    public int UserId { get; init; }

    public string OrderNumber { get; init; } = string.Empty;

    public DateTime OrderDate { get; init; }

    public decimal SubTotal { get; init; }

    public decimal? DeliveryFee { get; init; }

    public decimal? PackagingFee { get; init; }

    public decimal? TotalAmount { get; init; }

    public string OrderStatus { get; init; } = string.Empty;

    public List<OrderItemDto> OrderItems { get; init; } = [];

    public UserAddressDto? Address { get; init; }

    public string? PaymentMethod { get; init; }
}

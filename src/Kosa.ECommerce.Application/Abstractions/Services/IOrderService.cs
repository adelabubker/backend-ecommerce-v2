using Kosa.ECommerce.Application.DTOs.Orders;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Abstractions.Services;

public interface IOrderService
{
    Task<Result<IReadOnlyList<OrderDto>>> GetOrdersAsync(int userId, CancellationToken cancellationToken = default);

    Task<Result<OrderDetailsDto>> GetOrderDetailsAsync(int orderId, int? requestedBy = null, CancellationToken cancellationToken = default);

    Task<Result<OrderDto>> GetByOrderNumberAsync(string orderNumber, int? requestedBy = null, CancellationToken cancellationToken = default);

    Task<Result<OrderDto>> CreateOrderAsync(CreateOrderDto request, CancellationToken cancellationToken = default);
}

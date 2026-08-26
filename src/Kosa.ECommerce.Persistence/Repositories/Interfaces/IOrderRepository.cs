using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;

namespace Kosa.ECommerce.Persistence.Abstractions.Repositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IReadOnlyList<Order>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken cancellationToken = default);

    Task<Order?> GetByIdWithDetailsAsync(int orderId, CancellationToken cancellationToken = default);
}

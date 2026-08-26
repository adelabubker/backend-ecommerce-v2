using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;

namespace Kosa.ECommerce.Persistence.Abstractions.Repositories;

public interface ICartRepository : IGenericRepository<Cart>
{
    Task<Cart?> GetActiveCartByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}

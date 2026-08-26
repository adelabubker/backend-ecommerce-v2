using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;

namespace Kosa.ECommerce.Persistence.Abstractions.Repositories;

public interface IFavoriteRepository : IGenericRepository<Favorite>
{
    Task<IReadOnlyList<Favorite>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(int userId, int productId, CancellationToken cancellationToken = default);
}

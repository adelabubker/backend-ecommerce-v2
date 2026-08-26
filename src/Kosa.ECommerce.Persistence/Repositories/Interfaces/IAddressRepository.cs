using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;

namespace Kosa.ECommerce.Persistence.Abstractions.Repositories;

public interface IAddressRepository : IGenericRepository<UserAddress>
{
    Task<IReadOnlyList<UserAddress>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}

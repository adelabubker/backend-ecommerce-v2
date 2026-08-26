using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;

namespace Kosa.ECommerce.Persistence.Abstractions.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailOrPhoneAsync(string emailOrPhone, CancellationToken cancellationToken = default);

    Task<User?> GetByEmailOrPhoneExcludingUserAsync(string emailOrPhone, int excludeUserId, CancellationToken cancellationToken = default);
}

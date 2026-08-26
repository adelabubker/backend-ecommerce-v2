using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Context;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kosa.ECommerce.Persistence.Repositories.Implementation;

public sealed class AddressRepository : GenericRepository<UserAddress>, IAddressRepository
{
    public AddressRepository(KosaDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<UserAddress>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ThenBy(a => a.AddressId)
            .ToListAsync(cancellationToken);
    }
}

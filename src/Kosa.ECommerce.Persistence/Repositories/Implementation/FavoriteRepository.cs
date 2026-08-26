using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Context;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kosa.ECommerce.Persistence.Repositories.Implementation;

public sealed class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
{
    public FavoriteRepository(KosaDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Favorite>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Include(f => f.Product)
                .ThenInclude(p => p.ProductImages)
            .Where(f => f.UserId == userId)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int userId, int productId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(f => f.UserId == userId && f.ProductId == productId, cancellationToken);
    }
}

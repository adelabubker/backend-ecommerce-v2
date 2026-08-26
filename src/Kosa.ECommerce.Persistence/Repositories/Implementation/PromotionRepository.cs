using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Context;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kosa.ECommerce.Persistence.Repositories.Implementation;

public sealed class PromotionRepository : GenericRepository<Promotion>, IPromotionRepository
{
    public PromotionRepository(KosaDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Promotion>> GetActivePromotionsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return await DbSet
            .AsNoTracking()
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.StartDate <= today && p.EndDate >= today)
            .OrderBy(p => p.Title)
            .ToListAsync(cancellationToken);
    }
}

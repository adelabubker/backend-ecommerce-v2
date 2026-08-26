using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Context;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kosa.ECommerce.Persistence.Repositories.Implementation;

public sealed class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(KosaDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.CategoryName)
            .ToListAsync(cancellationToken);
    }
}

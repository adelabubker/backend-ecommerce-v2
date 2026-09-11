using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Context;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Kosa.ECommerce.Persistence.Repositories.Implementation;

public sealed class AppSettingRepository
    : GenericRepository<AppSetting>, IAppSettingRepository
{
    public AppSettingRepository(KosaDbContext context)
        : base(context)
    {
    }

    public async Task<AppSetting?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(
                s => s.SettingKey == key,
                cancellationToken);
    }
}
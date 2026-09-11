using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;

namespace Kosa.ECommerce.Persistence.Abstractions.Repositories;

public interface IAppSettingRepository : IGenericRepository<AppSetting>
{
    Task<AppSetting?> GetByKeyAsync(
        string key,
        CancellationToken cancellationToken = default);
}
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;

namespace Kosa.ECommerce.Persistence.Abstractions.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<IReadOnlyList<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
}

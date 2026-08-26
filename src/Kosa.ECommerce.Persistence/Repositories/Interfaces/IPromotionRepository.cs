using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Persistence.Repositories.Generic;

namespace Kosa.ECommerce.Persistence.Abstractions.Repositories;

public interface IPromotionRepository : IGenericRepository<Promotion>
{
    /// <summary>Promotions that are flagged active and whose date window includes today.</summary>
    Task<IReadOnlyList<Promotion>> GetActivePromotionsAsync(CancellationToken cancellationToken = default);
}

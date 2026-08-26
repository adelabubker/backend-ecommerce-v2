using Kosa.ECommerce.Application.DTOs.Promotions;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Abstractions.Services;

public interface IPromotionService
{
    Task<Result<IReadOnlyList<PromotionDto>>> GetActivePromotionsAsync(CancellationToken cancellationToken = default);
}

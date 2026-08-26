using AutoMapper;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Promotions;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Services;

public sealed class PromotionService : IPromotionService
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IMapper _mapper;

    public PromotionService(IPromotionRepository promotionRepository, IMapper mapper)
    {
        _promotionRepository = promotionRepository;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<PromotionDto>>> GetActivePromotionsAsync(CancellationToken cancellationToken = default)
    {
        var promotions = await _promotionRepository.GetActivePromotionsAsync(cancellationToken);
        return Result<IReadOnlyList<PromotionDto>>.Ok(_mapper.Map<IReadOnlyList<PromotionDto>>(promotions));
    }
}

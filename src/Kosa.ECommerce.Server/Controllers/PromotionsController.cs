using Kosa.ECommerce.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/promotions")]
public sealed class PromotionsController : ControllerBase
{
    private readonly IPromotionService _promotionService;

    public PromotionsController(IPromotionService promotionService)
    {
        _promotionService = promotionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetActivePromotions(CancellationToken cancellationToken)
    {
        var result = await _promotionService.GetActivePromotionsAsync(cancellationToken);
        return Ok(result);
    }
}

using Kosa.ECommerce.Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/promotions")]
public sealed class PromotionsController : ControllerBase
{
    private readonly IPromotionService _promotionService;
    private readonly ILogger<PromotionsController> _logger;

    public PromotionsController(
        IPromotionService promotionService,
        ILogger<PromotionsController> logger)
    {
        _promotionService = promotionService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetActivePromotions(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting active promotions.");

        var result = await _promotionService.GetActivePromotionsAsync(
            cancellationToken);

        return Ok(result);
    }
}
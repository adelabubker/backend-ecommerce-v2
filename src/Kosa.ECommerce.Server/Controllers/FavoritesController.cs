using System.Security.Claims;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Favorites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/favorites")]
public sealed class FavoritesController : ControllerBase
{
    private readonly IFavoriteService _favoriteService;
    private readonly ILogger<FavoritesController> _logger;

    public FavoritesController(
        IFavoriteService favoriteService,
        ILogger<FavoritesController> logger)
    {
        _favoriteService = favoriteService;
        _logger = logger;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetFavorites(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting favorites for user {UserId}.",
            CurrentUserId);

        var result = await _favoriteService.GetFavoritesAsync(
            CurrentUserId,
            cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddFavorite(
        [FromBody] AddFavoriteDto dto,
        CancellationToken cancellationToken)
    {
        if (dto.UserId != CurrentUserId)
        {
            _logger.LogWarning(
                "User {UserId} attempted to add a favorite for another user {RequestedUserId}.",
                CurrentUserId,
                dto.UserId);

            return Forbid();
        }

        _logger.LogInformation(
            "Adding product {ProductId} to favorites for user {UserId}.",
            dto.ProductId,
            CurrentUserId);

        var result = await _favoriteService.AddFavoriteAsync(
            dto,
            cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{favoriteId:int}")]
    public async Task<IActionResult> RemoveFavorite(
        int favoriteId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Removing favorite {FavoriteId} for user {UserId}.",
            favoriteId,
            CurrentUserId);

        var result = await _favoriteService.RemoveFavoriteAsync(
            favoriteId,
            CurrentUserId,
            cancellationToken);

        return Ok(result);
    }
}
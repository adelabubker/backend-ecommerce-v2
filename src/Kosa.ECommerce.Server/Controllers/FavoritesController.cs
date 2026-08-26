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

    public FavoritesController(IFavoriteService favoriteService)
    {
        _favoriteService = favoriteService;
    }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetFavorites(CancellationToken cancellationToken)
    {
        var result = await _favoriteService.GetFavoritesAsync(CurrentUserId, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteDto dto, CancellationToken cancellationToken)
    {
        if (dto.UserId != CurrentUserId)
        {
            return Forbid();
        }
        var result = await _favoriteService.AddFavoriteAsync(dto, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{favoriteId:int}")]
    public async Task<IActionResult> RemoveFavorite(int favoriteId, CancellationToken cancellationToken)
    {
        var result = await _favoriteService.RemoveFavoriteAsync(favoriteId, CurrentUserId, cancellationToken);
        return Ok(result);
    }
}

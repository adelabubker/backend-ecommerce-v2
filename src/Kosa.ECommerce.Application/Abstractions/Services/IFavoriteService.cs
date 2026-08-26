using Kosa.ECommerce.Application.DTOs.Favorites;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Abstractions.Services;

public interface IFavoriteService
{
    Task<Result<IReadOnlyList<FavoriteDto>>> GetFavoritesAsync(int userId, CancellationToken cancellationToken = default);

    Task<Result<FavoriteDto>> AddFavoriteAsync(AddFavoriteDto dto, CancellationToken cancellationToken = default);

    Task<Result<bool>> RemoveFavoriteAsync(int favoriteId, int? userId = null, CancellationToken cancellationToken = default);
}

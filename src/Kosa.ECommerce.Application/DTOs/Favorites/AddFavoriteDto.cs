using System.ComponentModel.DataAnnotations;

namespace Kosa.ECommerce.Application.DTOs.Favorites;

public sealed class AddFavoriteDto
{
    [Range(1, int.MaxValue)]
    public int UserId { get; init; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; init; }
}

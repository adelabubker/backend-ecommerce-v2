using System.ComponentModel.DataAnnotations;

namespace Kosa.ECommerce.Application.DTOs.Cart;

public sealed class UpdateCartItemQuantityDto
{
    [Range(1, int.MaxValue)]
    public int UserId { get; init; }

    [Range(1, int.MaxValue)]
    public int ProductId { get; init; }

    [Range(1, 999)]
    public int Quantity { get; init; }
}

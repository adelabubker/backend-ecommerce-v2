using Kosa.ECommerce.Application.DTOs.Cart;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Abstractions.Services;

public interface ICartService
{
    Task<Result<CartDto>> GetCartAsync(int userId, CancellationToken cancellationToken = default);

    Task<Result<CartDto>> AddItemAsync(AddCartItemDto request, CancellationToken cancellationToken = default);

    Task<Result<CartDto>> UpdateQuantityAsync(UpdateCartItemQuantityDto request, CancellationToken cancellationToken = default);

    Task<Result<bool>> RemoveItemAsync(int userId, int productId, CancellationToken cancellationToken = default);
}

using Kosa.ECommerce.Application.DTOs.Users;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Abstractions.Services;

public interface IUserProfileService
{
    Task<Result<UserProfileDto>> GetProfileAsync(int userId, CancellationToken cancellationToken = default);

    Task<Result<UserProfileDto>> UpdateProfileAsync(int userId, UpdateUserProfileDto request, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<UserAddressDto>>> GetAddressesAsync(int userId, CancellationToken cancellationToken = default);

    Task<Result<int>> SaveAddressAsync(int userId, UserAddressDto request, CancellationToken cancellationToken = default);

    Task<Result<int>> DeleteAddressAsync(int addressId, int? userId = null, CancellationToken cancellationToken = default);
}

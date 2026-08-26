using Kosa.ECommerce.Application.DTOs.Authentication;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Abstractions.Services;

public interface IAuthenticationService
{
    Task<Result<AuthUserDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);

    Task<Result<AuthUserDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);

    Task<Result<CurrentUserDto>> GetCurrentUserAsync(int userId, CancellationToken cancellationToken = default);
}

using Kosa.ECommerce.Persistence.Entities;

namespace Kosa.ECommerce.Application.Abstractions.Services;

/// <summary>
/// Issues signed JWT access tokens for authenticated users.
/// Implemented in the Infrastructure layer.
/// </summary>
public interface IJwtTokenGenerator
{
    string CreateToken(User user);
}

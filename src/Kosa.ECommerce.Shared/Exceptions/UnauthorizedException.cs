namespace Kosa.ECommerce.Shared.Exceptions;

/// <summary>
/// Signals an expected application error that maps to the HTTP status
/// code implied by its name. Thrown by application services and translated
/// into consistent error responses by the global exception middleware.
/// </summary>
public sealed class UnauthorizedException : Exception
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}

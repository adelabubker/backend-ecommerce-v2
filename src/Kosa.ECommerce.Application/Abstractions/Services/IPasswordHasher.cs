namespace Kosa.ECommerce.Application.Abstractions.Services;

/// <summary>
/// Abstraction over password hashing so the Application layer never depends
/// on a concrete algorithm. Implemented in the Infrastructure layer.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);

    bool Verify(string password, string hashedPassword);
}

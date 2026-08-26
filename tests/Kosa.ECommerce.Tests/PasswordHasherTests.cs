using Kosa.ECommerce.Infrastructure.Authentication;

namespace Kosa.ECommerce.Tests;

public sealed class PasswordHasherTests
{
    private readonly PasswordHasher _hasher = new();

    [Fact]
    public void HashAndVerify_RoundTrips()
    {
        var hash = _hasher.Hash("P@ssw0rd!123");

        Assert.NotEqual("P@ssw0rd!123", hash);
        Assert.True(_hasher.Verify("P@ssw0rd!123", hash));
    }

    [Fact]
    public void Verify_WrongPassword_Fails()
    {
        var hash = _hasher.Hash("correct-password");

        Assert.False(_hasher.Verify("wrong-password", hash));
    }

    [Fact]
    public void Hash_ProducesUniqueSalts()
    {
        var first = _hasher.Hash("same-password");
        var second = _hasher.Hash("same-password");

        Assert.NotEqual(first, second);
    }
}

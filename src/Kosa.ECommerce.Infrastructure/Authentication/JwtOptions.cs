namespace Kosa.ECommerce.Infrastructure.Authentication;

/// <summary>Configuration options bound from the "Jwt" configuration section.</summary>
public sealed class JwtOptions
{
    public string Key { get; set; } = string.Empty;

    public string Issuer { get; set; } = "KosaECommerce";

    public string Audience { get; set; } = "KosaECommerceClient";

    public int ExpiresInDays { get; set; } = 30;
}

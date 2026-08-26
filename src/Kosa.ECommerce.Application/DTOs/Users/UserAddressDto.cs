namespace Kosa.ECommerce.Application.DTOs.Users;

public sealed class UserAddressDto
{
    public int AddressId { get; init; }

    public int UserId { get; init; }

    public string AddressType { get; init; } = string.Empty;

    public string? AddressLine { get; init; }

    public string City { get; init; } = string.Empty;

    public string? State { get; init; }

    public string? PostalCode { get; init; }

    public string Country { get; init; } = string.Empty;

    public decimal Latitude { get; init; }

    public decimal Longitude { get; init; }

    public bool IsDefault { get; init; }
}

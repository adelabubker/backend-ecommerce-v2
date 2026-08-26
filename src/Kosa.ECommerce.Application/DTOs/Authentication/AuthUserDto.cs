namespace Kosa.ECommerce.Application.DTOs.Authentication;

public sealed class AuthUserDto
{
    public int UserId { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string EmailOrPhone { get; init; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public DateTime CreatedDate { get; init; }
}

namespace Kosa.ECommerce.Application.DTOs.Users;

public sealed class UserProfileDto
{
    public int UserId { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string? Email { get; init; }

    public string? Phone { get; init; }

    public string? ProfileImage { get; init; }

    public DateTime CreatedDate { get; init; }
}

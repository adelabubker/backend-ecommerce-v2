using System.ComponentModel.DataAnnotations;

namespace Kosa.ECommerce.Application.DTOs.Users;

public sealed class UpdateUserProfileDto
{
    [Required, MaxLength(150)]
    public string FullName { get; init; } = string.Empty;

    [EmailAddress, MaxLength(150)]
    public string? Email { get; init; }

    [MaxLength(20)]
    public string? Phone { get; init; }

    [MaxLength(500)]
    public string? ProfileImage { get; init; }
}

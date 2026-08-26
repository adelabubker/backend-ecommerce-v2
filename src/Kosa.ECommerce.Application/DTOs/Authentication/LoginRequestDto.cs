using System.ComponentModel.DataAnnotations;

namespace Kosa.ECommerce.Application.DTOs.Authentication;

public sealed class LoginRequestDto
{
    [Required, MaxLength(150)]
    public string EmailOrPhone { get; init; } = string.Empty;

    [Required]
    public string Password { get; init; } = string.Empty;
}

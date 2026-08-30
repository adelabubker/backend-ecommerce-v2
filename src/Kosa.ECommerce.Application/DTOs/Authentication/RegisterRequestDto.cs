using System.ComponentModel.DataAnnotations;

namespace Kosa.ECommerce.Application.DTOs.Authentication;

public sealed class RegisterRequestDto
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; init; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string EmailOrPhone { get; init; } = string.Empty;

    [Required]
    [MinLength(6)]
    [MaxLength(128)]
    public string Password { get; init; } = string.Empty;
}
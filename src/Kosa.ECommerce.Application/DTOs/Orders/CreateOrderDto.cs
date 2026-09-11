using System.ComponentModel.DataAnnotations;

namespace Kosa.ECommerce.Application.DTOs.Orders;

public sealed class CreateOrderDto
{
    [Range(1, int.MaxValue)]
    public int UserId { get; init; }

    [Range(1, int.MaxValue)]
    public int AddressId { get; init; }

    [Required, MaxLength(50)]
    public string PaymentMethod { get; init; } = "Cash";

    
}

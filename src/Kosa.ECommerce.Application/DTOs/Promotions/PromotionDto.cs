namespace Kosa.ECommerce.Application.DTOs.Promotions;

public sealed class PromotionDto
{
    public int PromotionId { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public int CategoryId { get; init; }

    public string CategoryName { get; init; } = string.Empty;

    public int DiscountPercent { get; init; }

    public string BannerImage { get; init; } = string.Empty;

    public DateOnly StartDate { get; init; }

    public DateOnly EndDate { get; init; }
}

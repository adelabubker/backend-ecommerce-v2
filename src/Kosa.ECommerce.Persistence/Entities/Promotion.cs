namespace Kosa.ECommerce.Persistence.Entities;

public class Promotion
{
    public int PromotionId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int CategoryId { get; set; }

    public int DiscountPercent { get; set; }

    public string BannerImage { get; set; } = null!;

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public bool IsActive { get; set; }

    public Category Category { get; set; } = null!;
}

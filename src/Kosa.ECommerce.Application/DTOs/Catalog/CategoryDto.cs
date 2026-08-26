namespace Kosa.ECommerce.Application.DTOs.Catalog;

public sealed class CategoryDto
{
    public int CategoryId { get; init; }

    public string CategoryName { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string? ImageUrl { get; init; }

    public string? Description { get; init; }
}

namespace Kosa.ECommerce.Application.DTOs.Catalog;

public sealed class ProductDto
{
    public int ProductId { get; init; }

    public int CategoryId { get; init; }

    public string CategoryName { get; init; } = string.Empty;

    public string Slug { get; init; } = string.Empty;

    public string ProductName { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Unit { get; init; } = string.Empty;

    public decimal Price { get; init; }

    public string? ImageUrl { get; init; }

    public IReadOnlyList<string> GalleryImageUrls { get; init; } = [];
}

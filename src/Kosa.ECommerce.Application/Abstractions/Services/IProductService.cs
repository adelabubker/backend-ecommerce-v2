using Kosa.ECommerce.Application.DTOs.Catalog;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Abstractions.Services;

public interface IProductService
{
    Task<Result<IReadOnlyList<ProductDto>>> GetProductsAsync(CancellationToken cancellationToken = default);

    Task<Result<ProductDto>> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default);

    Task<Result<ProductDto>> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<ProductDto>>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<ProductDto>>> SearchProductsByNameAsync(string name, CancellationToken cancellationToken = default);
}

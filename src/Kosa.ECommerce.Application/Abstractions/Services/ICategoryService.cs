using Kosa.ECommerce.Application.DTOs.Catalog;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Abstractions.Services;

public interface ICategoryService
{
    Task<Result<IReadOnlyList<CategoryDto>>> GetCategoriesAsync(CancellationToken cancellationToken = default);
}

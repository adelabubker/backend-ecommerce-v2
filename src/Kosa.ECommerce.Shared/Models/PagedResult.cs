using Kosa.ECommerce.Shared.Constants;

namespace Kosa.ECommerce.Shared.Models;

/// <summary>
/// Standard paginated result envelope used across the catalog APIs.
/// </summary>
public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];

    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = AppConstants.DefaultPageSize;

    public int TotalCount { get; init; }

    public int TotalPages => PageSize <= 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasNextPage => Page < TotalPages;
}

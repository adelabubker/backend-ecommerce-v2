using AutoMapper;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Catalog;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<CategoryDto>>> GetCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        try
        {
            var categories = await _categoryRepository
                .GetActiveCategoriesAsync(cancellationToken);

            return Result<IReadOnlyList<CategoryDto>>.Ok(
                _mapper.Map<IReadOnlyList<CategoryDto>>(categories));
        }
        catch
        {
            throw;
        }
    }
}
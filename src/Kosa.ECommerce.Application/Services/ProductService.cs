using AutoMapper;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Catalog;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Shared.Exceptions;
using Kosa.ECommerce.Shared.Helpers;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<ProductDto>>> GetProductsAsync(CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetActiveProductsAsync(cancellationToken);
        return Result<IReadOnlyList<ProductDto>>.Ok(_mapper.Map<IReadOnlyList<ProductDto>>(products));
    }

    public async Task<Result<ProductDto>> GetProductByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdWithImagesAsync(productId, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException("Product was not found.");
        }

        return Result<ProductDto>.Ok(_mapper.Map<ProductDto>(product));
    }

    public async Task<Result<ProductDto>> GetProductBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetActiveProductsAsync(cancellationToken);
        var product = products.FirstOrDefault(p => SlugHelper.CreateSlug(p.ProductName) == slug);
        if (product is null)
        {
            throw new NotFoundException("Product was not found.");
        }

        return Result<ProductDto>.Ok(_mapper.Map<ProductDto>(product));
    }

    public async Task<Result<IReadOnlyList<ProductDto>>> GetProductsByCategoryIdAsync(int categoryId, CancellationToken cancellationToken = default)
    {
        var products = await _productRepository.GetByCategoryIdAsync(categoryId, cancellationToken);
        return Result<IReadOnlyList<ProductDto>>.Ok(_mapper.Map<IReadOnlyList<ProductDto>>(products));
    }

    public async Task<Result<IReadOnlyList<ProductDto>>> SearchProductsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<IReadOnlyList<ProductDto>>.Ok([]);
        }

        var products = await _productRepository.SearchByNameAsync(name.Trim(), cancellationToken);
        return Result<IReadOnlyList<ProductDto>>.Ok(_mapper.Map<IReadOnlyList<ProductDto>>(products));
    }
}

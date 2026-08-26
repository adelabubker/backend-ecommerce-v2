using AutoMapper;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Favorites;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Services;

public sealed class FavoriteService : IFavoriteService
{
    private readonly IFavoriteRepository _favoriteRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public FavoriteService(IFavoriteRepository favoriteRepository, IProductRepository productRepository, IMapper mapper)
    {
        _favoriteRepository = favoriteRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<FavoriteDto>>> GetFavoritesAsync(int userId, CancellationToken cancellationToken = default)
    {
        var favorites = await _favoriteRepository.GetByUserIdAsync(userId, cancellationToken);
        return Result<IReadOnlyList<FavoriteDto>>.Ok(_mapper.Map<IReadOnlyList<FavoriteDto>>(favorites));
    }

    public async Task<Result<FavoriteDto>> AddFavoriteAsync(AddFavoriteDto dto, CancellationToken cancellationToken = default)
    {
        if (await _favoriteRepository.ExistsAsync(dto.UserId, dto.ProductId, cancellationToken))
        {
            throw new ConflictException("This product is already in the user's favorites.");
        }

        var product = await _productRepository.GetByIdWithImagesAsync(dto.ProductId, cancellationToken);
        if (product is null)
        {
            throw new NotFoundException("Product was not found.");
        }

        var entity = new Favorite
        {
            UserId = dto.UserId,
            ProductId = dto.ProductId,
            CreatedDate = DateTime.Now
        };

        await _favoriteRepository.AddAsync(entity, cancellationToken);
        await _favoriteRepository.SaveChangesAsync(cancellationToken);

        var result = new FavoriteDto
        {
            FavoriteId = entity.FavoriteId,
            UserId = entity.UserId,
            ProductId = entity.ProductId,
            ProductName = product.ProductName,
            ProductPrice = product.Price,
            ProductImageUrl = product.ProductImages.FirstOrDefault()?.ImageUrl,
            CreatedDate = entity.CreatedDate
        };

        return Result<FavoriteDto>.Ok(result, "Favorite added.");
    }

    public async Task<Result<bool>> RemoveFavoriteAsync(int favoriteId, int? userId = null, CancellationToken cancellationToken = default)
    {
        var favorite = await _favoriteRepository.GetByIdAsync(favoriteId, cancellationToken);
        if (favorite is null)
        {
            throw new NotFoundException("Favorite was not found.");
        }
        if (userId.HasValue && favorite.UserId != userId.Value)
        {
            throw new UnauthorizedException("You cannot remove another user's favorite.");
        }

        _favoriteRepository.Delete(favorite);
        await _favoriteRepository.SaveChangesAsync(cancellationToken);

        return Result<bool>.Ok(true, "Favorite removed.");
    }
}

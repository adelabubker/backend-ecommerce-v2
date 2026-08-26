using Kosa.ECommerce.Application.Services;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Moq;

namespace Kosa.ECommerce.Tests;

public sealed class FavoriteServiceTests
{
    [Fact]
    public async Task AddFavorite_Success_ReturnsMappedFavorite()
    {
        var favorites = new Mock<IFavoriteRepository>();
        var products = new Mock<IProductRepository>();

        favorites.Setup(r => r.ExistsAsync(1, 7, It.IsAny<CancellationToken>())).ReturnsAsync(false);
        products.Setup(r => r.GetByIdWithImagesAsync(7, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Product
            {
                ProductId = 7,
                ProductName = "Olive Oil",
                Price = 25m,
                ProductImages = [new ProductImage { ImageUrl = "https://img/olive.jpg" }]
            });
        favorites.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new FavoriteService(favorites.Object, products.Object, TestHelpers.CreateMapper());
        var result = await service.AddFavoriteAsync(new Kosa.ECommerce.Application.DTOs.Favorites.AddFavoriteDto
        {
            UserId = 1,
            ProductId = 7
        });

        Assert.True(result.Success);
        Assert.Equal("Olive Oil", result.Data!.ProductName);
        Assert.Equal(25m, result.Data.ProductPrice);
        favorites.Verify(r => r.AddAsync(It.Is<Favorite>(f => f.UserId == 1 && f.ProductId == 7), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddFavorite_Duplicate_ThrowsConflict()
    {
        var favorites = new Mock<IFavoriteRepository>();
        var products = new Mock<IProductRepository>();
        favorites.Setup(r => r.ExistsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var service = new FavoriteService(favorites.Object, products.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<ConflictException>(() => service.AddFavoriteAsync(new Kosa.ECommerce.Application.DTOs.Favorites.AddFavoriteDto { UserId = 1, ProductId = 7 }));
    }

    [Fact]
    public async Task RemoveFavorite_NotOwned_ThrowsUnauthorized()
    {
        var favorites = new Mock<IFavoriteRepository>();
        var products = new Mock<IProductRepository>();
        favorites.Setup(r => r.GetByIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Favorite { FavoriteId = 3, UserId = 999, ProductId = 7 });

        var service = new FavoriteService(favorites.Object, products.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.RemoveFavoriteAsync(3, 1));
    }

    [Fact]
    public async Task RemoveFavorite_Missing_ThrowsNotFound()
    {
        var favorites = new Mock<IFavoriteRepository>();
        var products = new Mock<IProductRepository>();
        favorites.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((Favorite?)null);

        var service = new FavoriteService(favorites.Object, products.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<NotFoundException>(() => service.RemoveFavoriteAsync(3, 1));
    }
}

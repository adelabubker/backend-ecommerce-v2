using Kosa.ECommerce.Application.Services;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Moq;

namespace Kosa.ECommerce.Tests;

public sealed class ProductServiceTests
{
    [Fact]
    public async Task GetProducts_ReturnsMappedDtos()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetActiveProductsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>
            {
                new()
                {
                    ProductId = 1,
                    ProductName = "Tomato",
                    Description = "Fresh",
                    Unit = "kg",
                    Price = 5.5m,
                    IsActive = true,
                    Category = new Category { CategoryId = 2, CategoryName = "Vegetables" },
                    ProductImages = [new ProductImage { ImageUrl = "https://img/tomato.jpg" }]
                }
            });

        var service = new ProductService(repository.Object, TestHelpers.CreateMapper());
        var result = await service.GetProductsAsync();

        Assert.True(result.Success);
        var dto = Assert.Single(result.Data!);
        Assert.Equal("Tomato", dto.ProductName);
        Assert.Equal("Vegetables", dto.CategoryName);
        Assert.Equal("tomato", dto.Slug);
        Assert.Equal("https://img/tomato.jpg", dto.ImageUrl);
        Assert.Single(dto.GalleryImageUrls);
    }

    [Fact]
    public async Task GetProductById_MissingProduct_ThrowsNotFound()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByIdWithImagesAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var service = new ProductService(repository.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetProductByIdAsync(99));
    }

    [Fact]
    public async Task GetProductsByCategoryId_ReturnsOnlyThatCategory()
    {
        var repository = new Mock<IProductRepository>();
        repository.Setup(r => r.GetByCategoryIdAsync(3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Product>
            {
                new() { ProductId = 10, ProductName = "Apple", CategoryId = 3, IsActive = true }
            });

        var service = new ProductService(repository.Object, TestHelpers.CreateMapper());
        var result = await service.GetProductsByCategoryIdAsync(3);

        Assert.Single(result.Data!);
        Assert.Equal(3, result.Data![0].CategoryId);
    }

    [Fact]
    public async Task SearchProducts_EmptyName_ReturnsEmptyList()
    {
        var repository = new Mock<IProductRepository>();
        var service = new ProductService(repository.Object, TestHelpers.CreateMapper());

        var result = await service.SearchProductsByNameAsync("   ");

        Assert.Empty(result.Data!);
        repository.Verify(r => r.SearchByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

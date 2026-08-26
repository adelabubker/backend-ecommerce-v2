using Kosa.ECommerce.Application.Services;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Moq;

namespace Kosa.ECommerce.Tests;

public sealed class CategoryServiceTests
{
    [Fact]
    public async Task GetCategories_ReturnsOnlyActive_Mapped()
    {
        var repository = new Mock<ICategoryRepository>();
        repository.Setup(r => r.GetActiveCategoriesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>
            {
                new() { CategoryId = 1, CategoryName = "Vegetables", IsActive = true },
                new() { CategoryId = 2, CategoryName = "Fruits", IsActive = true }
            });

        var service = new CategoryService(repository.Object, TestHelpers.CreateMapper());
        var result = await service.GetCategoriesAsync();

        Assert.Equal(2, result.Data!.Count);
        Assert.Equal("vegetables", result.Data![0].Slug);
        Assert.Equal("Vegetables", result.Data![0].CategoryName);
    }
}

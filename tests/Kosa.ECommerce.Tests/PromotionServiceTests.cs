using Kosa.ECommerce.Application.Services;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Moq;

namespace Kosa.ECommerce.Tests;

public sealed class PromotionServiceTests
{
    [Fact]
    public async Task GetActivePromotions_ReturnsMappedDtos()
    {
        var repository = new Mock<IPromotionRepository>();
        repository.Setup(r => r.GetActivePromotionsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Promotion>
            {
                new()
                {
                    PromotionId = 1,
                    Title = "Summer Sale",
                    Description = "20% off",
                    CategoryId = 1,
                    DiscountPercent = 20,
                    BannerImage = "https://img/banner.png",
                    StartDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
                    EndDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                    IsActive = true,
                    Category = new Category { CategoryId = 1, CategoryName = "Fruits" }
                }
            });

        var service = new PromotionService(repository.Object, TestHelpers.CreateMapper());
        var result = await service.GetActivePromotionsAsync();

        var dto = Assert.Single(result.Data!);
        Assert.Equal("Summer Sale", dto.Title);
        Assert.Equal("Fruits", dto.CategoryName);
    }
}

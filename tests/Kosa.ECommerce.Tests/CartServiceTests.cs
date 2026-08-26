using Kosa.ECommerce.Application.DTOs.Cart;
using Kosa.ECommerce.Application.Services;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Moq;

namespace Kosa.ECommerce.Tests;

public sealed class CartServiceTests
{
    private static Product Product(int id = 1, string name = "Tomato", decimal price = 5m) => new()
    {
        ProductId = id,
        ProductName = name,
        Price = price,
        IsActive = true,
        ProductImages = []
    };

    private static Cart CartWith(int userId, params CartItem[] items) => new()
    {
        CartId = 1,
        UserId = userId,
        IsOrdered = false,
        CartItems = items.ToList()
    };

    [Fact]
    public async Task GetCart_NoCart_ReturnsEmptyCart()
    {
        var carts = new Mock<ICartRepository>();
        var products = new Mock<IProductRepository>();
        carts.Setup(r => r.GetActiveCartByUserIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync((Cart?)null);

        var service = new CartService(carts.Object, products.Object, TestHelpers.CreateMapper());
        var result = await service.GetCartAsync(5);

        Assert.True(result.Success);
        Assert.Equal(5, result.Data!.UserId);
        Assert.Empty(result.Data.Items);
    }

    [Fact]
    public async Task AddItem_CreatesCartWhenNoneExists()
    {
        var carts = new Mock<ICartRepository>();
        var products = new Mock<IProductRepository>();
        var tomato = Product();

        carts.SetupSequence(r => r.GetActiveCartByUserIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Cart?)null)
            .ReturnsAsync(CartWith(5, new CartItem { CartItemId = 1, ProductId = 1, Quantity = 2, Price = 5m, Product = tomato }));
        products.Setup(r => r.GetByIdWithImagesAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(tomato);
        carts.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new CartService(carts.Object, products.Object, TestHelpers.CreateMapper());
        var result = await service.AddItemAsync(new AddCartItemDto { UserId = 5, ProductId = 1, Quantity = 2 });

        Assert.True(result.Success);
        Assert.Equal(10m, result.Data!.Total);
        carts.Verify(r => r.AddAsync(It.Is<Cart>(c => c.UserId == 5), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddItem_QuantityZero_ThrowsValidation()
    {
        var carts = new Mock<ICartRepository>();
        var products = new Mock<IProductRepository>();
        var service = new CartService(carts.Object, products.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<ValidationException>(() => service.AddItemAsync(new AddCartItemDto { UserId = 5, ProductId = 1, Quantity = 0 }));
    }

    [Fact]
    public async Task UpdateQuantity_UpdatesExistingItem()
    {
        var carts = new Mock<ICartRepository>();
        var products = new Mock<IProductRepository>();
        var tomato = Product();

        carts.SetupSequence(r => r.GetActiveCartByUserIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(CartWith(5, new CartItem { CartItemId = 1, ProductId = 1, Quantity = 1, Price = 5m, Product = tomato }))
            .ReturnsAsync(CartWith(5, new CartItem { CartItemId = 1, ProductId = 1, Quantity = 4, Price = 5m, Product = tomato }));
        carts.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new CartService(carts.Object, products.Object, TestHelpers.CreateMapper());
        var result = await service.UpdateQuantityAsync(new UpdateCartItemQuantityDto { UserId = 5, ProductId = 1, Quantity = 4 });

        Assert.True(result.Success);
        Assert.Equal(4, result.Data!.Items[0].Quantity);
        Assert.Equal(20m, result.Data.Total);
    }

    [Fact]
    public async Task RemoveItem_EmptyCartGetsDeleted()
    {
        var carts = new Mock<ICartRepository>();
        var products = new Mock<IProductRepository>();
        var tomato = Product();
        var cart = CartWith(5, new CartItem { CartItemId = 1, ProductId = 1, Quantity = 1, Price = 5m, Product = tomato });
        carts.Setup(r => r.GetActiveCartByUserIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(cart);
        carts.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new CartService(carts.Object, products.Object, TestHelpers.CreateMapper());
        var result = await service.RemoveItemAsync(5, 1);

        Assert.True(result.Success);
        carts.Verify(r => r.Delete(It.Is<Cart>(c => c.CartId == cart.CartId)), Times.Once);
    }
}

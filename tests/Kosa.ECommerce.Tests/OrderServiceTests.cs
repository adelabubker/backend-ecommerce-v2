using Kosa.ECommerce.Application.DTOs.Orders;
using Kosa.ECommerce.Application.Services;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Moq;

namespace Kosa.ECommerce.Tests;

public sealed class OrderServiceTests
{
    private static Cart CartForCheckout() => new()
    {
        CartId = 1,
        UserId = 5,
        IsOrdered = false,
        CartItems =
        [
            new CartItem { CartItemId = 1, ProductId = 1, Quantity = 2, Price = 10m, Product = new Product { ProductId = 1, ProductName = "Dates", Price = 10m } }
        ]
    };

    [Fact]
    public async Task CreateOrder_ComputesTotalsServerSide_AndClosesCart()
    {
        var orders = new Mock<IOrderRepository>();
        var carts = new Mock<ICartRepository>();
        var addresses = new Mock<IAddressRepository>();

        addresses.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UserAddress { AddressId = 10, UserId = 5 });
        carts.Setup(r => r.GetActiveCartByUserIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(CartForCheckout());
        orders.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new OrderService(orders.Object, carts.Object, addresses.Object, TestHelpers.CreateMapper());
        var result = await service.CreateOrderAsync(new CreateOrderDto
        {
            UserId = 5,
            AddressId = 10,
            PaymentMethod = "Card",
            DeliveryFee = 3m,
            PackagingFee = 2m
        });

        Assert.True(result.Success);
        Assert.Equal(20m, result.Data!.SubTotal);
        Assert.Equal(25m, result.Data.Total);
        Assert.Equal("Pending", result.Data.Status);
        orders.Verify(r => r.AddAsync(It.Is<Order>(o => o.OrderNumber.StartsWith("KOSA-") && o.TotalAmount == 25m), It.IsAny<CancellationToken>()), Times.Once);
        carts.Verify(r => r.Update(It.Is<Cart>(c => c.IsOrdered)), Times.Once);
    }

    [Fact]
    public async Task CreateOrder_EmptyCart_ThrowsValidation()
    {
        var orders = new Mock<IOrderRepository>();
        var carts = new Mock<ICartRepository>();
        var addresses = new Mock<IAddressRepository>();

        addresses.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(new UserAddress { AddressId = 10, UserId = 5 });
        carts.Setup(r => r.GetActiveCartByUserIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(new Cart { CartId = 1, UserId = 5, CartItems = [] });

        var service = new OrderService(orders.Object, carts.Object, addresses.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateOrderAsync(new CreateOrderDto { UserId = 5, AddressId = 10 }));
    }

    [Fact]
    public async Task CreateOrder_AddressOfAnotherUser_ThrowsValidation()
    {
        var orders = new Mock<IOrderRepository>();
        var carts = new Mock<ICartRepository>();
        var addresses = new Mock<IAddressRepository>();

        addresses.Setup(r => r.GetByIdAsync(10, It.IsAny<CancellationToken>())).ReturnsAsync(new UserAddress { AddressId = 10, UserId = 999 });
        carts.Setup(r => r.GetActiveCartByUserIdAsync(5, It.IsAny<CancellationToken>())).ReturnsAsync(CartForCheckout());

        var service = new OrderService(orders.Object, carts.Object, addresses.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<ValidationException>(() => service.CreateOrderAsync(new CreateOrderDto { UserId = 5, AddressId = 10 }));
    }

    [Fact]
    public async Task GetOrderDetails_NotOwned_ThrowsUnauthorized()
    {
        var orders = new Mock<IOrderRepository>();
        var carts = new Mock<ICartRepository>();
        var addresses = new Mock<IAddressRepository>();

        orders.Setup(r => r.GetByIdWithDetailsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Order { OrderId = 1, UserId = 999, OrderNumber = "KOSA-1" });

        var service = new OrderService(orders.Object, carts.Object, addresses.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<UnauthorizedException>(() => service.GetOrderDetailsAsync(1, 5));
    }

    [Fact]
    public async Task GetByOrderNumber_Missing_ThrowsNotFound()
    {
        var orders = new Mock<IOrderRepository>();
        var carts = new Mock<ICartRepository>();
        var addresses = new Mock<IAddressRepository>();

        orders.Setup(r => r.GetByOrderNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Order?)null);

        var service = new OrderService(orders.Object, carts.Object, addresses.Object, TestHelpers.CreateMapper());

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetByOrderNumberAsync("KOSA-404", 5));
    }

    [Fact]
    public async Task GetOrders_ReturnsUserOrders()
    {
        var orders = new Mock<IOrderRepository>();
        var carts = new Mock<ICartRepository>();
        var addresses = new Mock<IAddressRepository>();

        orders.Setup(r => r.GetByUserIdAsync(5, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Order>
            {
                new()
                {
                    OrderId = 1,
                    UserId = 5,
                    OrderNumber = "KOSA-1",
                    OrderDate = DateTime.Now,
                    SubTotal = 20m,
                    TotalAmount = 25m,
                    OrderStatus = "Pending",
                    OrderItems = [new OrderItem { ProductId = 1, Quantity = 2, Price = 10m, Product = new Product { ProductName = "Dates" } }]
                }
            });

        var service = new OrderService(orders.Object, carts.Object, addresses.Object, TestHelpers.CreateMapper());
        var result = await service.GetOrdersAsync(5);

        var dto = Assert.Single(result.Data!);
        Assert.Equal("KOSA-1", dto.OrderNumber);
        Assert.Equal("Dates", dto.Items[0].ProductName);
    }
}

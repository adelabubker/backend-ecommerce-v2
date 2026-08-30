using AutoMapper;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Orders;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Services;

public sealed class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IAddressRepository addressRepository,
        IMapper mapper)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _addressRepository = addressRepository;
        _mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<OrderDto>>> GetOrdersAsync(int userId, CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId, cancellationToken);
        return Result<IReadOnlyList<OrderDto>>.Ok(_mapper.Map<IReadOnlyList<OrderDto>>(orders));
    }

    public async Task<Result<OrderDetailsDto>> GetOrderDetailsAsync(int orderId, int? requestedBy = null, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdWithDetailsAsync(orderId, cancellationToken);
        if (order is null)
        {
            throw new NotFoundException("Order was not found.");
        }
        if (requestedBy.HasValue && order.UserId != requestedBy.Value)
        {
            throw new UnauthorizedException("You are not allowed to view this order.");
        }

        return Result<OrderDetailsDto>.Ok(_mapper.Map<OrderDetailsDto>(order));
    }

    public async Task<Result<OrderDto>> GetByOrderNumberAsync(string orderNumber, int? requestedBy = null, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByOrderNumberAsync(orderNumber, cancellationToken);
        if (order is null)
        {
            throw new NotFoundException("Order was not found.");
        }
        if (requestedBy.HasValue && order.UserId != requestedBy.Value)
        {
            throw new UnauthorizedException("You are not allowed to view this order.");
        }

        return Result<OrderDto>.Ok(_mapper.Map<OrderDto>(order));
    }

    public async Task<Result<OrderDto>> CreateOrderAsync(CreateOrderDto request, CancellationToken cancellationToken = default)
    {
        if (request.UserId <= 0 || request.AddressId <= 0)
        {
            throw new ValidationException("User and address are required.");
        }

        // The delivery address must belong to the ordering user.
        var address = await _addressRepository.GetByIdAsync(request.AddressId, cancellationToken);
        if (address is null)
        {
            throw new ValidationException("Address was not found.");
        }
        if (address.UserId != request.UserId)
        {
            throw new ValidationException("Address does not belong to the current user.");
        }

        var cart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId, cancellationToken);
        if (cart is null || cart.CartItems.Count == 0)
        {
            throw new ValidationException("Cannot create an order from an empty cart.");
        }

        // Totals are computed server-side from the cart; the client only supplies fees.
        var now = DateTime.Now;
        var subTotal = cart.CartItems.Sum(ci => ci.Quantity * (ci.Product?.Price ?? ci.Price));
        var totalAmount = subTotal + request.DeliveryFee + request.PackagingFee;

        var order = new Order
        {
            UserId = request.UserId,
            AddressId = request.AddressId,
            OrderNumber = $"KOSA-{DateTime.UtcNow:yyyyMMddHHmmssfff}{Random.Shared.Next(1000)}",
            OrderDate = now,
            SubTotal = subTotal,
            DeliveryFee = request.DeliveryFee,
            PackagingFee = request.PackagingFee,
            TotalAmount = totalAmount,
            OrderStatus = "Pending",
            OrderItems = cart.CartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Price = ci.Product?.Price ?? ci.Price
            }).ToList(),
            Payments =
            [
                new Payment
                {
                    PaymentMethod = request.PaymentMethod,
                    Amount = totalAmount,
                    PaymentStatus = "Pending",
                    PaymentDate = now
                }
            ]
        };

        await _orderRepository.AddAsync(order, cancellationToken);

        // The cart is closed (checked out) and preserved for audit history.
        cart.IsOrdered = true;
        _cartRepository.Update(cart);

        await _orderRepository.SaveChangesAsync(cancellationToken);

        return Result<OrderDto>.Ok(_mapper.Map<OrderDto>(order), "Order created successfully.");
    }
}

using System.Security.Claims;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        IOrderService orderService,
        ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetOrders(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting orders for user {UserId}.",
            CurrentUserId);

        var result = await _orderService.GetOrdersAsync(
            CurrentUserId,
            cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("by-number/{orderNumber}")]
    public async Task<IActionResult> GetOrderByNumber(
        string orderNumber,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting order {OrderNumber} for user {UserId}.",
            orderNumber,
            CurrentUserId);

        var result = await _orderService.GetByOrderNumberAsync(
            orderNumber,
            CurrentUserId,
            cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetOrderDetails(
        int orderId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting order details for order {OrderId} and user {UserId}.",
            orderId,
            CurrentUserId);

        var result = await _orderService.GetOrderDetailsAsync(
            orderId,
            CurrentUserId,
            cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderDto request,
        CancellationToken cancellationToken)
    {
        if (request.UserId != CurrentUserId)
        {
            _logger.LogWarning(
                "User {UserId} attempted to create an order for another user {RequestedUserId}.",
                CurrentUserId,
                request.UserId);

            return Forbid();
        }

        _logger.LogInformation(
            "Creating order for user {UserId}.",
            CurrentUserId);

        var result = await _orderService.CreateOrderAsync(
            request,
            cancellationToken);

        _logger.LogInformation(
            "Order created successfully for user {UserId}.",
            CurrentUserId);

        return Ok(result);
    }
}
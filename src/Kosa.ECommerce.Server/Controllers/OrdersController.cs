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

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
    {
        var result = await _orderService.GetOrdersAsync(CurrentUserId, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("by-number/{orderNumber}")]
    public async Task<IActionResult> GetOrderByNumber(string orderNumber, CancellationToken cancellationToken)
    {
        var result = await _orderService.GetByOrderNumberAsync(orderNumber, CurrentUserId, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetOrderDetails(int orderId, CancellationToken cancellationToken)
    {
        var result = await _orderService.GetOrderDetailsAsync(orderId, CurrentUserId, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto request, CancellationToken cancellationToken)
    {
        if (request.UserId != CurrentUserId)
        {
            return Forbid();
        }
        var result = await _orderService.CreateOrderAsync(request, cancellationToken);
        return Ok(result);
    }
}

using System.Security.Claims;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kosa.ECommerce.Server.Controllers;

[ApiController]
[Route("api/cart")]
public sealed class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    private int CurrentUserId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCart(CancellationToken cancellationToken)
    {
        var result = await _cartService.GetCartAsync(CurrentUserId, cancellationToken);
        return Ok(result); //
    }

    [Authorize]
    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddCartItemDto request, CancellationToken cancellationToken)
    {
        if (request.UserId != CurrentUserId)
        {
            return Forbid();
        }
        var result = await _cartService.AddItemAsync(request, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPut("items/quantity")]
    public async Task<IActionResult> UpdateQuantity([FromBody] UpdateCartItemQuantityDto request, CancellationToken cancellationToken)
    {
        if (request.UserId != CurrentUserId)
        {
            return Forbid();
        }
        var result = await _cartService.UpdateQuantityAsync(request, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpDelete("items/{productId:int}")]
    public async Task<IActionResult> RemoveItem(int productId, CancellationToken cancellationToken)
    {
        var result = await _cartService.RemoveItemAsync(CurrentUserId, productId, cancellationToken);
        return Ok(result);
    }
}

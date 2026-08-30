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
    private readonly ILogger<CartController> _logger;

    public CartController(
        ICartService cartService,
        ILogger<CartController> logger)
    {
        _cartService = cartService;
        _logger = logger;
    }

    private int CurrentUserId =>
        int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetCart(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Getting cart for user {UserId}.",
            CurrentUserId);

        var result = await _cartService.GetCartAsync(
            CurrentUserId,
            cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPost("items")]
    public async Task<IActionResult> AddItem(
        [FromBody] AddCartItemDto request,
        CancellationToken cancellationToken)
    {
        if (request.UserId != CurrentUserId)
        {
            _logger.LogWarning(
                "User {UserId} attempted to add a cart item for another user {RequestedUserId}.",
                CurrentUserId,
                request.UserId);

            return Forbid();
        }

        _logger.LogInformation(
            "Adding product {ProductId} to cart for user {UserId}.",
            request.ProductId,
            CurrentUserId);

        var result = await _cartService.AddItemAsync(
            request,
            cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpPut("items/quantity")]
    public async Task<IActionResult> UpdateQuantity(
        [FromBody] UpdateCartItemQuantityDto request,
        CancellationToken cancellationToken)
    {
        if (request.UserId != CurrentUserId)
        {
            _logger.LogWarning(
                "User {UserId} attempted to update cart quantity for another user {RequestedUserId}.",
                CurrentUserId,
                request.UserId);

            return Forbid();
        }

        _logger.LogInformation(
            "Updating cart item for user {UserId} to quantity {Quantity}.",
            CurrentUserId,
            request.Quantity);

        var result = await _cartService.UpdateQuantityAsync(
            request,
            cancellationToken);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("items/{productId:int}")]
    public async Task<IActionResult> RemoveItem(
        int productId,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Removing product {ProductId} from cart for user {UserId}.",
            productId,
            CurrentUserId);

        var result = await _cartService.RemoveItemAsync(
            CurrentUserId,
            productId,
            cancellationToken);

        return Ok(result);
    }
}
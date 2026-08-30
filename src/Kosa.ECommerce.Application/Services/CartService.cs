using AutoMapper;
using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.DTOs.Cart;
using Kosa.ECommerce.Persistence.Abstractions.Repositories;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Exceptions;
using Kosa.ECommerce.Shared.Models;

namespace Kosa.ECommerce.Application.Services;

public sealed class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> GetCartAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await _cartRepository
                .GetActiveCartByUserIdAsync(
                    userId,
                    cancellationToken);

            if (cart is null)
            {
                return Result<CartDto>.Ok(
                    new CartDto
                    {
                        CartId = 0,
                        UserId = userId,
                        Items = []
                    },
                    "Cart is empty.");
            }

            return Result<CartDto>.Ok(
                _mapper.Map<CartDto>(cart));
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<CartDto>> AddItemAsync(
        AddCartItemDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (request.Quantity <= 0)
            {
                throw new ValidationException(
                    "Quantity must be greater than zero.");
            }

            var product = await _productRepository
                .GetByIdWithImagesAsync(
                    request.ProductId,
                    cancellationToken);

            if (product is null)
            {
                throw new NotFoundException(
                    "Product was not found.");
            }

            var cart = await _cartRepository
                .GetActiveCartByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            if (cart is null)
            {
                cart = new Cart
                {
                    UserId = request.UserId,
                    CreatedDate = DateTime.UtcNow,
                    IsOrdered = false
                };

                await _cartRepository.AddAsync(
                    cart,
                    cancellationToken);
            }

            var existingItem = cart.CartItems
                .FirstOrDefault(i =>
                    i.ProductId == request.ProductId);

            if (existingItem is null)
            {
                cart.CartItems.Add(
                    new CartItem
                    {
                        ProductId = request.ProductId,
                        Quantity = request.Quantity,
                        Price = product.Price
                    });
            }
            else
            {
                existingItem.Quantity += request.Quantity;
                existingItem.Price = product.Price;
            }

            await _cartRepository.SaveChangesAsync(
                cancellationToken);

            var updatedCart = await _cartRepository
                .GetActiveCartByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            return Result<CartDto>.Ok(
                _mapper.Map<CartDto>(updatedCart),
                "Item added to cart.");
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<CartDto>> UpdateQuantityAsync(
        UpdateCartItemQuantityDto request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (request.Quantity <= 0)
            {
                throw new ValidationException(
                    "Quantity must be greater than zero.");
            }

            var cart = await _cartRepository
                .GetActiveCartByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            if (cart is null)
            {
                throw new NotFoundException(
                    "Cart was not found.");
            }

            var item = cart.CartItems
                .FirstOrDefault(i =>
                    i.ProductId == request.ProductId);

            if (item is null)
            {
                throw new NotFoundException(
                    "Cart item was not found.");
            }

            item.Quantity = request.Quantity;

            await _cartRepository.SaveChangesAsync(
                cancellationToken);

            var updatedCart = await _cartRepository
                .GetActiveCartByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            return Result<CartDto>.Ok(
                _mapper.Map<CartDto>(updatedCart),
                "Cart quantity updated.");
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<bool>> RemoveItemAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var cart = await _cartRepository
                .GetActiveCartByUserIdAsync(
                    userId,
                    cancellationToken);

            if (cart is null)
            {
                throw new NotFoundException(
                    "Cart was not found.");
            }

            var item = cart.CartItems
                .FirstOrDefault(i =>
                    i.ProductId == productId);

            if (item is null)
            {
                throw new NotFoundException(
                    "Cart item was not found.");
            }

            _cartRepository.DeleteCartItem(item);

            await _cartRepository.SaveChangesAsync(
                cancellationToken);

            return Result<bool>.Ok(
                true,
                "Item removed from cart.");
        }
        catch
        {
            throw;
        }
    }
}
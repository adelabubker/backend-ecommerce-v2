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
    private readonly IAppSettingRepository _appSettingRepository;
    private readonly IMapper _mapper;

    public CartService(
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IAppSettingRepository appSettingRepository,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _productRepository = productRepository;
        _appSettingRepository = appSettingRepository;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> GetCartAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        var cart = await _cartRepository
            .GetActiveCartByUserIdAsync(
                userId,
                cancellationToken);

        var (deliveryFee, packagingFee) =
            await GetCartFeesAsync(cancellationToken);

        if (cart is null)
        {
            return Result<CartDto>.Ok(
                new CartDto
                {
                    CartId = 0,
                    UserId = userId,
                    Items = [],
                    DeliveryFee = deliveryFee,
                    PackagingFee = packagingFee
                },
                "Cart is empty.");
        }

        var cartDto = _mapper.Map<CartDto>(cart);

        return Result<CartDto>.Ok(
            new CartDto
            {
                CartId = cartDto.CartId,
                UserId = cartDto.UserId,
                Items = cartDto.Items,
                DeliveryFee = deliveryFee,
                PackagingFee = packagingFee
            });
    }

    public async Task<Result<CartDto>> AddItemAsync(
        AddCartItemDto request,
        CancellationToken cancellationToken = default)
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
            await BuildCartDtoAsync(
                updatedCart!,
                cancellationToken),
            "Item added to cart.");
    }

    public async Task<Result<CartDto>> UpdateQuantityAsync(
        UpdateCartItemQuantityDto request,
        CancellationToken cancellationToken = default)
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
            await BuildCartDtoAsync(
                updatedCart!,
                cancellationToken),
            "Cart quantity updated.");
    }

    public async Task<Result<bool>> RemoveItemAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default)
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

    private async Task<CartDto> BuildCartDtoAsync(
        Cart cart,
        CancellationToken cancellationToken)
    {
        var mappedCart = _mapper.Map<CartDto>(cart);

        var (deliveryFee, packagingFee) =
            await GetCartFeesAsync(cancellationToken);

        return new CartDto
        {
            CartId = mappedCart.CartId,
            UserId = mappedCart.UserId,
            Items = mappedCart.Items,
            DeliveryFee = deliveryFee,
            PackagingFee = packagingFee
        };
    }

    private async Task<(decimal DeliveryFee, decimal PackagingFee)> GetCartFeesAsync(
        CancellationToken cancellationToken)
    {
        var deliveryFeeSetting =
            await _appSettingRepository.GetByKeyAsync(
                "DeliveryFee",
                cancellationToken);

        var packagingFeeSetting =
            await _appSettingRepository.GetByKeyAsync(
                "PackagingFee",
                cancellationToken);

        if (deliveryFeeSetting is null ||
            packagingFeeSetting is null)
        {
            throw new ValidationException(
                "Cart pricing settings are not configured.");
        }

        if (!decimal.TryParse(
                deliveryFeeSetting.SettingValue,
                out var deliveryFee) ||
            !decimal.TryParse(
                packagingFeeSetting.SettingValue,
                out var packagingFee))
        {
            throw new ValidationException(
                "Cart pricing settings are invalid.");
        }

        return (deliveryFee, packagingFee);
    }
}

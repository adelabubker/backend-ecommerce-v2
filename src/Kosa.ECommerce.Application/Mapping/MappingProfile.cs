using AutoMapper;
using Kosa.ECommerce.Application.DTOs.Authentication;
using Kosa.ECommerce.Application.DTOs.Cart;
using Kosa.ECommerce.Application.DTOs.Catalog;
using Kosa.ECommerce.Application.DTOs.Favorites;
using Kosa.ECommerce.Application.DTOs.Orders;
using Kosa.ECommerce.Application.DTOs.Promotions;
using Kosa.ECommerce.Application.DTOs.Users;
using Kosa.ECommerce.Persistence.Entities;
using Kosa.ECommerce.Shared.Helpers;

namespace Kosa.ECommerce.Application.Mapping;

/// <summary>
/// Central AutoMapper configuration for Kosa.ECommerce.
/// Maps persistence entities to application DTOs and back where writes
/// are required, always ignoring server-controlled members (ids, user ids).
/// Property names now follow one canonical contract end-to-end:
/// database PascalCase (ProductId, ProductName, ImageUrl) → DTO PascalCase
/// → JSON camelCase (productId, productName, imageUrl) → Angular camelCase.
/// </summary>
public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ----- Users / Authentication -----
        CreateMap<User, UserProfileDto>();

        CreateMap<User, AuthUserDto>()
            .ForMember(dest => dest.EmailOrPhone, opt => opt.MapFrom(src => src.Email ?? src.Phone))
            .ForMember(dest => dest.Token, opt => opt.Ignore());

        CreateMap<User, CurrentUserDto>()
            .ForMember(dest => dest.EmailOrPhone, opt => opt.MapFrom(src => src.Email ?? src.Phone));

        // ----- Addresses -----
        CreateMap<UserAddress, UserAddressDto>();

        CreateMap<UserAddressDto, UserAddress>()
            .ForMember(dest => dest.AddressId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.Orders, opt => opt.Ignore())
            .ForMember(dest => dest.User, opt => opt.Ignore());

        // ----- Catalog -----
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => SlugHelper.CreateSlug(src.CategoryName)));

        CreateMap<Product, ProductDto>()
    .ForMember(dest => dest.Slug,
        opt => opt.MapFrom(src => SlugHelper.CreateSlug(src.ProductName)))
    .ForMember(dest => dest.CategoryName,
        opt => opt.MapFrom(src =>
            src.Category == null ? string.Empty : src.Category.CategoryName))
    .ForMember(dest => dest.ImageUrl,
        opt => opt.MapFrom(src =>
            src.ProductImages
                .Select(i => i.ImageUrl)
                .FirstOrDefault()))
    .ForMember(dest => dest.GalleryImageUrls,
        opt => opt.MapFrom(src =>
            src.ProductImages
                .Select(i => i.ImageUrl)
                .Skip(1)
                .ToList()));

        // ----- Promotions -----
        CreateMap<Promotion, PromotionDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category == null ? string.Empty : src.Category.CategoryName));

        // ----- Cart -----
        CreateMap<Cart, CartDto>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems));

        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product == null ? "Removed Item" : src.Product.ProductName))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product == null ? null : src.Product.ProductImages.Select(i => i.ImageUrl).FirstOrDefault()));

        // ----- Favorites -----
        CreateMap<Favorite, FavoriteDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product == null ? string.Empty : src.Product.ProductName))
            .ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product == null ? 0 : src.Product.Price))
            .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product == null ? null : src.Product.ProductImages.Select(i => i.ImageUrl).FirstOrDefault()));

        // ----- Orders -----
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.EstimatedDeliveryAt, opt => opt.MapFrom(src => DateTimeHelper.GetEstimatedDeliveryTime(src.OrderDate)))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.OrderStatus))
            .ForMember(dest => dest.DeliveryFee, opt => opt.MapFrom(src => src.DeliveryFee ?? 0))
            .ForMember(dest => dest.PackagingFee, opt => opt.MapFrom(src => src.PackagingFee ?? 0))
            .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.TotalAmount ?? src.SubTotal))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderItems));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product == null ? "Removed Item" : src.Product.ProductName));

        CreateMap<Order, OrderDetailsDto>()
            .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.Payments.Select(p => p.PaymentMethod).FirstOrDefault()))
            .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
            .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
    }
}

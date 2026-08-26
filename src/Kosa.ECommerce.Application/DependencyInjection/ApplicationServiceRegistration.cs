using Kosa.ECommerce.Application.Abstractions.Services;
using Kosa.ECommerce.Application.Mapping;
using Kosa.ECommerce.Application.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Kosa.ECommerce.Application.DependencyInjection;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserProfileService, UserProfileService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IFavoriteService, FavoriteService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPromotionService, PromotionService>();

        return services;
    }
}

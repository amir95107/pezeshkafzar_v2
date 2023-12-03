using Microsoft.AspNetCore.Mvc.Infrastructure;
using Pezeshkafzar_v2.Services;

namespace Pezeshkafzar_v2.Repositories;
public static class RegisterRepositories
{
    public static void Handle(WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddScoped<IBlogRepository, BlogRepository>();
        builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
        builder.Services.AddScoped<IHomeRepository, HomeRepository>();
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IBrandRepository, BrandRepository>();
        builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
        builder.Services.AddScoped<IProductGroupRepository, ProductGroupRepository>();
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Pezeshkafzar_v2.Repositories;

namespace Pezeshkafzar_v2.Services;
public static class RegisterServices
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
        builder.Services.AddScoped<IPageRepository, PageRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();
        builder.Services.AddScoped<IDeliverWaysRepository, DeliverWaysRepository>();
        builder.Services.AddScoped<ISliderRepository, SliderRepository>();
        builder.Services.AddScoped<IAddressRepository, AddressRepository>();
    }
}
using Application.Logic;
using Application.Logic.AuthService;
using Application.Logic.CategoryService;
using Application.Logic.UserService;
using Application.Logic.ProductService;
using Application.Logic.SubCategoryService;
using Infrastructure.Repository.Interface;
using Infrastructure.Repository;
using Infrastructure.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Application.Logic.ProductVariantService;
using Application.Logic.CartService;
using Application.Logic.CartItemService;
using Application.Logic.OrderService;
using Infrastructure.GenericRepository;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // Register repositories and services
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();

        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ICategoryService, CategoryService>();

        services.AddScoped<ISubCategoryRepository, SubCategoryRepository>();
        services.AddScoped<ISubCategoryService, SubCategoryService>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();

        services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
        services.AddScoped<IProductVariantService, ProductVariantService>();

        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ICartService, CartService>();

        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<ICartItemService, CartItemService>();

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService>();

        services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<TokenService>();
        services.AddScoped<IUnit, Unit>();
        services.AddScoped<PdfService>();

    }
}

using ShopOrder.Domain.Interfaces.Users;
using ShopOrder.Infrastructure.Business.Users;
using ShopOrder.Infrastructure.Data.Users;
using ShopOrder.Services.Interfaces.Users;
using ShopOrder.Infrastructure.Data;
using ShopOrder.Infrastructure.Business.Orders;
using ShopOrder.Services.Interfaces.Orders;
using ShopOrder.Infrastructure.Data.Orders;
using ShopOrder.Domain.Interfaces.Orders;
using ShopOrder.Services.Interfaces.OrderDetails;
using ShopOrder.Domain.Interfaces.OrderDetails;
using ShopOrder.Infrastructure.Data.OrderDetails;
using ShopOrder.Infrastructure.Business.OrderDetails;

namespace ShopOrder.Utilities
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterDI(this IServiceCollection services, IConfiguration configuration)
        {
            // Db
            services.AddShopOrderDbContext(configuration.GetConnectionString("ShopConnection") ?? "DefaultConnectionString");

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();

            // Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Logger
            services.AddLogging(builder =>
            {
                builder.AddFile(configuration.GetSection("Logging"));
            });

            return services;
        }
    }
}
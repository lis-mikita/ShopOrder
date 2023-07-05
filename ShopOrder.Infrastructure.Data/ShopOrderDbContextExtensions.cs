using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ShopOrder.Infrastructure.Data
{
    public static class ShopOrderDbContextExtensions
    {
        /// <summary>
        /// Adds ShopOrderDbContext to the specified IServiceCollection. Uses the SqlServer database provider.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">Set to override the default of "..".</param>
        /// <returns>An IServiceCollection that can be used to add more services.</returns>
        public static IServiceCollection AddShopOrderDbContext(
            this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ShopOrderDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
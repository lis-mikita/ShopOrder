using ShopOrder.Domain.Core.Infrastructure.Enums;
using ShopOrder.Domain.Core.Models.Orders;

namespace ShopOrder.Domain.Interfaces.Orders
{
    public interface IOrderRepository
    {
        Task<Order?> CreateOrderAsync(Order order);
        Task<Order?> UpdateOrderAsync(int id, Order order);
        Task<bool> DeleteOrderAsync(int id);
        Task<Order?> GetOrderAsync(int id);
        Task<Order?> GetOrderWithOrderDetailsAsync(int id);
        Task<IEnumerable<Order>?> GetOrdersAsync();
        Task<OrderStatus?> CheckStatusOrderAsync(int id);
        Task<bool> CheckUserExistsAsync(int id);
    }
}
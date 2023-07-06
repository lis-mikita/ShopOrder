using ShopOrder.Domain.Core.Models.OrderDetails;

namespace ShopOrder.Domain.Interfaces.OrderDetails
{
    public interface IOrderDetailRepository
    {
        Task<OrderDetail?> CreateOrderDetailAsync(OrderDetail orderDetail);
        Task<OrderDetail?> UpdateOrderDetailAsync(int id, OrderDetail orderDetail);
        Task<bool> DeleteOrderDetailAsync(int id);
        Task<OrderDetail?> GetOrderDetailAsync(int id);
        Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync();
        Task<bool> CheckOrderExistsAsync(int id);
    }
}
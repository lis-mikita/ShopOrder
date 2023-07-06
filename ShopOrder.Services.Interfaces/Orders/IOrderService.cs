using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Messages;
using ShopOrder.Domain.Core.Messages.Orders;
using ShopOrder.Domain.Core.Models.Orders;

namespace ShopOrder.Services.Interfaces.Orders
{
    public interface IOrderService
    {
        Task<OrderResponse> CreateOrderAsync(Order order, ModelStateDictionary modelState);
        Task<OrderResponse> GetOrderAsync(int id);
        Task<OrderResponse> UpdateOrderAsync(int id, Order order, ModelStateDictionary modelState);
        Task<BaseResponse> DeleteOrderAsync(int id);
        Task<OrdersResponse> GetOrdersAsync();
        Task<OrderResponse> GetOrderWithOrderDetailsAsync(int id);
    }
}

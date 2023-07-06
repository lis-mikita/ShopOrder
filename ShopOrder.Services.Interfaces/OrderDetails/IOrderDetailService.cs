using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Messages;
using ShopOrder.Domain.Core.Messages.OrderDetails;
using ShopOrder.Domain.Core.Models.OrderDetails;

namespace ShopOrder.Services.Interfaces.OrderDetails
{
    public interface IOrderDetailService
    {
        Task<OrderDetailResponse> CreateOrderDetailAsync(OrderDetail orderDetail, ModelStateDictionary modelState);
        Task<OrderDetailResponse> GetOrderDetailAsync(int id);
        Task<OrderDetailResponse> UpdateOrderDetailAsync(int id, OrderDetail orderDetail, ModelStateDictionary modelState);
        Task<BaseResponse> DeleteOrderDetailAsync(int id);
        Task<OrderDetailsResponse> GetOrderDetailsAsync();
    }
}

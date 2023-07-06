using ShopOrder.Domain.Core.Models.Orders;

namespace ShopOrder.Domain.Core.Messages.Orders
{
    public class OrderResponse : BaseResponse
    {
        public Order? Order { get; set; }
    }
}
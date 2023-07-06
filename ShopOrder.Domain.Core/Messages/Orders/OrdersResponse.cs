using ShopOrder.Domain.Core.Models.Orders;

namespace ShopOrder.Domain.Core.Messages.Orders
{
    public class OrdersResponse : BaseResponse
    {
        public IEnumerable<Order>? Orders { get; set; }
    }
}

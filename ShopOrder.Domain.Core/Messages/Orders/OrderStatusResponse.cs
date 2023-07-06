using ShopOrder.Domain.Core.Infrastructure.Enums;

namespace ShopOrder.Domain.Core.Messages.Orders
{
    public class OrderStatusResponse : BaseResponse
    {
        public OrderStatus? OrderStatus { get; set; }
    }
}

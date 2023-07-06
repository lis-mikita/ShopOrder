using ShopOrder.Domain.Core.Models.OrderDetails;

namespace ShopOrder.Domain.Core.Messages.OrderDetails
{
    public class OrderDetailsResponse : BaseResponse
    {
        public IEnumerable<OrderDetail>? OrderDetails { get; set; }
    }
}
using ShopOrder.Domain.Core.Models.OrderDetails;

namespace ShopOrder.Domain.Core.Messages.OrderDetails
{
    public class OrderDetailResponse : BaseResponse
    {
        public OrderDetail? OrderDetail { get; set; }
    }
}
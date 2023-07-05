// Ignore Spelling: DTO

using ShopOrder.Domain.Core.Infrastructure.Enums;
using ShopOrder.Domain.Core.Models.OrderDetails;

namespace ShopOrder.Domain.Core.DTO.Orders
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public OrderStatus? Status { get; set; }
        public string? DeliveryAddress { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
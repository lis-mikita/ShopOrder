using ShopOrder.Domain.Core.Infrastructure.Enums;
using ShopOrder.Domain.Core.Models.OrderDetails;
using ShopOrder.Domain.Core.Models.Users;

namespace ShopOrder.Domain.Core.Models.Orders
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public OrderStatus? Status { get; set; }
        public string? DeliveryAddress { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
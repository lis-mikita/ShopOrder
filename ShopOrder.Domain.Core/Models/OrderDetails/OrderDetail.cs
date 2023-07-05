using ShopOrder.Domain.Core.Models.Orders;

namespace ShopOrder.Domain.Core.Models.OrderDetails
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
        public virtual Order? Order { get; set; }
    }
}
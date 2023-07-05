// Ignore Spelling: DTO

using ShopOrder.Domain.Core.Models.Orders;

namespace ShopOrder.Domain.Core.DTO.OrderDetails
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public virtual Order? Order { get; set; }
    }
}
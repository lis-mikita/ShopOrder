using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ShopOrder.Domain.Core.Infrastructure.Enums;
using ShopOrder.Domain.Core.Models.OrderDetails;
using ShopOrder.Domain.Core.Models.Users;
using static ShopOrder.Domain.Core.Infrastructure.Constants;

namespace ShopOrder.Domain.Core.Models.Orders
{
    public class Order
    {
        [JsonIgnore]
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int UserId { get; set; }
        public DateTime? OrderDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = Validation.Orders.TotalAmountNegative)]
        public decimal? TotalAmount { get; set; }
        public OrderStatus? Status { get; set; }

        [MaxLength(60, ErrorMessage = Validation.Orders.DeliveryAddressMaxLength)]
        public string? DeliveryAddress { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}

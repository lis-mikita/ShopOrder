using ShopOrder.Domain.Core.Models.Orders;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static ShopOrder.Domain.Core.Infrastructure.Constants;

namespace ShopOrder.Domain.Core.Models.OrderDetails
{
    public class OrderDetail
    {
        [JsonIgnore]
        [Required]
        public int OrderDetailId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = Validation.OrderDetails.ProductNameMaxLength)]
        public string? ProductName { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = Validation.OrderDetails.QuantityNegative)]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = Validation.OrderDetails.PriceNegative)]
        public decimal Price { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = Validation.OrderDetails.SubtotalNegative)]
        public decimal Subtotal { get; set; }

        [JsonIgnore]
        public virtual Order? Order { get; set; }
    }
}

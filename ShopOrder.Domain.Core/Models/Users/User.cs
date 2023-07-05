using ShopOrder.Domain.Core.Models.Orders;

namespace ShopOrder.Domain.Core.Models.Users
{
    public class User
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

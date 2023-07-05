// Ignore Spelling: DTO

using ShopOrder.Domain.Core.Models.Orders;

namespace ShopOrder.Domain.Core.DTO.Users
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
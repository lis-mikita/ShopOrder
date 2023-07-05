using ShopOrder.Domain.Core.Models.Orders;
using System.ComponentModel.DataAnnotations;
using static ShopOrder.Domain.Core.Infrastructure.Constants;

namespace ShopOrder.Domain.Core.Models.Users
{
    public class User
    {
        [Required]
        public int UserId { get; set; }

        [MaxLength(30, ErrorMessage = Validation.Users.FirstNameMaxLength)]
        public string? FirstName { get; set; }

        [MaxLength(30, ErrorMessage = Validation.Users.LastNameMaxLength)]
        public string? LastName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = Validation.Users.EmailMaxLength)]
        [EmailAddress(ErrorMessage = Validation.Users.EmailError)]
        public string? Email { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = Validation.Users.PasswordMaxLength)]
        [MinLength(8, ErrorMessage = Validation.Users.PasswordMinLength)]
        public string? Password { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
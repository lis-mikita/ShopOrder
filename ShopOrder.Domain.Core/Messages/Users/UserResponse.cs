using ShopOrder.Domain.Core.Models.Users;

namespace ShopOrder.Domain.Core.Messages.Users
{
    public class UserResponse : BaseResponse
    {
        public User? User { get; set; }
    }
}

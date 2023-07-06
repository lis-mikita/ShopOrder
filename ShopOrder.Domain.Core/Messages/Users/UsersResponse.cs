using ShopOrder.Domain.Core.Models.Users;

namespace ShopOrder.Domain.Core.Messages.Users
{
    public class UsersResponse : BaseResponse
    {
        public IEnumerable<User>? Users { get; set; }
    }
}
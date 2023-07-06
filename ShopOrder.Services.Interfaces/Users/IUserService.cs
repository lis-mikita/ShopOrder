using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Messages;
using ShopOrder.Domain.Core.Messages.Users;
using ShopOrder.Domain.Core.Models.Users;

namespace ShopOrder.Services.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserResponse> CreateUserAsync(User user, ModelStateDictionary modelState);
        Task<UserResponse> GetUserAsync(int id);
        Task<UserResponse> UpdateUserAsync(int id, User user, ModelStateDictionary modelState);
        Task<BaseResponse> DeleteUserAsync(int id);
        Task<UsersResponse> GetUsersAsync();
        Task<UserResponse> GetUserWithOrdersAsync(int id);
    }
}
using ShopOrder.Domain.Core.Models.Users;

namespace ShopOrder.Domain.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<bool> CheckIfSameEmailExistsAsync(string email);
        Task<User?> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
        Task<User?> GetUserAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserWithOrdersAsync(int id);
    }
}
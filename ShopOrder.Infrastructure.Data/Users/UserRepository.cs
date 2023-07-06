using Microsoft.EntityFrameworkCore;
using ShopOrder.Domain.Core.Models.Users;
using ShopOrder.Domain.Interfaces.Users;

namespace ShopOrder.Infrastructure.Data.Users
{
    public class UserRepository : IUserRepository
    {
        private const int NO_CHANGES_SAVED_COUNT = 0;
        private readonly ShopOrderDbContext _db;

        public UserRepository(ShopOrderDbContext dbContext) => _db = dbContext;

        public async Task<bool> CheckIfSameEmailExistsAsync(string email)
        {
            return await _db.Users.AnyAsync(u => u.Email != null && u.Email.ToUpper() == email.ToUpper());
        }

        public async Task<User?> CreateUserAsync(User user)
        {
            await _db.Users.AddAsync(user);

            return (await _db.SaveChangesAsync()) > NO_CHANGES_SAVED_COUNT ? user : throw new DbUpdateException();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            if (user is null)
            {
                return false;
            }

            _db.Users.Remove(user);
            return (await _db.SaveChangesAsync()) > NO_CHANGES_SAVED_COUNT ? true : throw new DbUpdateException();
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = await _db.Users.FindAsync(id);
            if (existingUser is null)
            {
                return null;
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Orders = user.Orders;
            existingUser.RegistrationDate = user.RegistrationDate;

            return (await _db.SaveChangesAsync()) > NO_CHANGES_SAVED_COUNT ? existingUser : throw new DbUpdateException();
        }

        public async Task<User?> GetUserAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User?> GetUserWithOrdersAsync(int id)
        {
            return await _db.Users.Include(u => u.Orders).FirstAsync(u => u.UserId == id);
        }
    }
}
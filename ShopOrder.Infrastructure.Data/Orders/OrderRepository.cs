using Microsoft.EntityFrameworkCore;
using ShopOrder.Domain.Core.Infrastructure.Enums;
using ShopOrder.Domain.Core.Models.Orders;
using ShopOrder.Domain.Interfaces.Orders;

namespace ShopOrder.Infrastructure.Data.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private const int NO_CHANGES_SAVED_COUNT = 0;
        private readonly ShopOrderDbContext _db;
        public OrderRepository(ShopOrderDbContext dbContext) => _db = dbContext;

        public async Task<OrderStatus?> CheckStatusOrderAsync(int id)
        {
            var order = await _db.Orders.FindAsync(id);

            if (order is null)
            {
                return null;
            }

            return order.Status;
        }

        public async Task<bool> CheckUserExistsAsync(int id)
        {
            return await _db.Users.AnyAsync(u => u.UserId == id);
        }

        public async Task<Order?> CreateOrderAsync(Order order)
        {
            await _db.Orders.AddAsync(order);

            return (await _db.SaveChangesAsync()) > NO_CHANGES_SAVED_COUNT ? order : throw new DbUpdateException();
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order is null)
            {
                return false;
            }

            _db.Orders.Remove(order);
            return (await _db.SaveChangesAsync()) > NO_CHANGES_SAVED_COUNT ? true : throw new DbUpdateException();
        }

        public async Task<Order?> GetOrderAsync(int id)
        {
            return await _db.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>?> GetOrdersAsync()
        {
            return await _db.Orders.ToListAsync();
        }

        public async Task<Order?> GetOrderWithOrderDetailsAsync(int id)
        {
            return await _db.Orders.Include(o => o.OrderDetails).FirstAsync(o => o.OrderId == id);
        }

        public async Task<Order?> UpdateOrderAsync(int id, Order order)
        {
            var existingOrder = await _db.Orders.FindAsync(id);
            if (existingOrder is null)
            {
                return null;
            }

            existingOrder.OrderDate = order.OrderDate;
            existingOrder.TotalAmount = order.TotalAmount;
            existingOrder.Status = order.Status;
            existingOrder.DeliveryAddress = order.DeliveryAddress;
            existingOrder.OrderDetails = order.OrderDetails;

            return (await _db.SaveChangesAsync()) > NO_CHANGES_SAVED_COUNT ? existingOrder : throw new DbUpdateException();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using ShopOrder.Domain.Core.Models.OrderDetails;
using ShopOrder.Domain.Interfaces.OrderDetails;

namespace ShopOrder.Infrastructure.Data.OrderDetails
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private const int NO_CHANGES_SAVED_COUNT = 0;
        private readonly ShopOrderDbContext _db;

        public OrderDetailRepository(ShopOrderDbContext dbContext) => _db = dbContext;

        public async Task<bool> CheckOrderExistsAsync(int id)
        {
            return await _db.Orders.AnyAsync(o => o.OrderId == id);
        }

        public async Task<OrderDetail?> CreateOrderDetailAsync(OrderDetail orderDetail)
        {
            await _db.OrderDetails.AddAsync(orderDetail);

            return (await _db.SaveChangesAsync()) > NO_CHANGES_SAVED_COUNT ? orderDetail : throw new DbUpdateException();
        }

        public async Task<bool> DeleteOrderDetailAsync(int id)
        {
            var orderDetail = await _db.OrderDetails.FindAsync(id);
            if (orderDetail is null)
            {
                return false;
            }

            _db.OrderDetails.Remove(orderDetail);
            return (await _db.SaveChangesAsync()) > NO_CHANGES_SAVED_COUNT ? true : throw new DbUpdateException();
        }

        public async Task<OrderDetail?> GetOrderDetailAsync(int id)
        {
            return await _db.OrderDetails.FindAsync(id);
        }

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsAsync()
        {
            return await _db.OrderDetails.ToListAsync();
        }

        public async Task<OrderDetail?> UpdateOrderDetailAsync(int id, OrderDetail orderDetail)
        {
            var existingOrderDetail = await _db.OrderDetails.FindAsync(id);
            if (existingOrderDetail is null)
            {
                return null;
            }

            existingOrderDetail.ProductName = orderDetail.ProductName;
            existingOrderDetail.Price = orderDetail.Price;
            existingOrderDetail.Quantity = orderDetail.Quantity;

            return (await _db.SaveChangesAsync()) > NO_CHANGES_SAVED_COUNT ? existingOrderDetail : throw new DbUpdateException();
        }
    }
}
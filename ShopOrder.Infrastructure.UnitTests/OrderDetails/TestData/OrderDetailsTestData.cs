using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Models.OrderDetails;

namespace ShopOrder.Infrastructure.UnitTests.OrderDetails.TestData
{
    public class OrderDetailsTestData
    {
        private const char TestChar = '-';
        private const int CharsCount = 10;

        public class CreateOrderDetailAsync_TestData
        {
            public List<OrderDetail> OrderDetails = new()
            {
                new OrderDetail
                {
                    OrderId = 1,
                    ProductName = new string(TestChar, CharsCount),
                    Quantity = 2,
                    Price = 10,
                    Subtotal = 20
                }
            };

            public ModelStateDictionary ModelState = new();
        }

        public class GetOrderDetailAsync_TestData
        {
            public List<OrderDetail> OrderDetails = new()
            {
                new OrderDetail
                {
                    OrderDetailId = 1,
                    OrderId = 1,
                    ProductName = new string(TestChar, CharsCount),
                    Quantity = 2,
                    Price = 10,
                    Subtotal = 20
                },
                new OrderDetail
                {
                    OrderDetailId = 3,
                    OrderId = 1,
                    ProductName = new string(TestChar, CharsCount),
                    Quantity = 3,
                    Price = 10,
                    Subtotal = 30
                }
            };
        }

        public class UpdateOrderDetailAsync_TestData
        {
            public List<OrderDetail> OrderDetails = new()
            {
                new OrderDetail
                {
                    OrderId = 1,
                    ProductName = new string(TestChar, CharsCount),
                    Quantity = 2,
                    Price = 10,
                    Subtotal = 20
                }
            };

            public ModelStateDictionary ModelState = new();

            public int Id = 1;
        }

        public class DeleteOrderDetailAsync_TestData
        {
            public int Id = 1;
        }

        public class GetOrderDetailsAsync_TestData
        {
            public List<OrderDetail> OrderDetails = new()
            {
                new OrderDetail
                {
                    OrderDetailId = 1,
                    OrderId = 1,
                    ProductName = new string(TestChar, CharsCount),
                    Quantity = 2,
                    Price = 10,
                    Subtotal = 20
                }
            };
        }
    }
}
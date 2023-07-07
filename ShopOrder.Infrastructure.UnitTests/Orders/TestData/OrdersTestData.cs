using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Models.Orders;
using ShopOrder.Domain.Core.Infrastructure.Enums;
using ShopOrder.Domain.Core.Models.OrderDetails;

namespace ShopOrder.Infrastructure.UnitTests.Orders.TestData
{
    static class OrdersTestData
    {
        private const char TestChar = '-';
        private const int CharsCount = 10;

        public class CreateOrderAsync_TestData
        {
            public List<Order> Orders = new()
            {
                new Order
                {
                    UserId = 1,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    Status = OrderStatus.New,
                    DeliveryAddress = new string(TestChar, CharsCount)
                }
            };

            public ModelStateDictionary ModelState = new();
        }

        public class GetOrderAsync_TestData
        {
            public List<Order> Orders = new()
            {
                new Order
                {
                    OrderId = 1,
                    UserId = 1,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    Status = OrderStatus.New,
                    DeliveryAddress = new string(TestChar, CharsCount)
                },
                new Order
                {
                    OrderId = 3,
                    UserId = 1,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    Status = OrderStatus.New,
                    DeliveryAddress = new string(TestChar, CharsCount)
                }
            };
        }

        public class UpdateOrderAsync_TestData
        {
            public List<Order> Orders = new()
            {
                new Order
                {
                    UserId = 1,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    Status = OrderStatus.New,
                    DeliveryAddress = new string(TestChar, CharsCount)
                }
            };

            public ModelStateDictionary ModelState = new();

            public int Id = 1;
        }

        public class DeleteOrderAsync_TestData
        {
            public int Id = 1;
        }

        public class GetOrdersAsync_TestData
        {
            public List<Order> Orders = new()
            {
                new Order
                {
                    OrderId = 1,
                    UserId = 1,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    Status = OrderStatus.New,
                    DeliveryAddress = new string(TestChar, CharsCount)
                }
            };
        }

        public class GetOrderWithOrderDetailsAsync_TestData
        {
            public List<Order> Orders = new()
            {
                new Order
                {
                    OrderId = 1,
                    UserId = 1,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0,
                    Status = OrderStatus.New,
                    DeliveryAddress = new string(TestChar, CharsCount),
                    OrderDetails = new[]
                    {
                        new OrderDetail
                        {
                            OrderId = 1,
                            ProductName = new string(TestChar, CharsCount),
                            Quantity = 1,
                            Price = 1,
                            Subtotal = 1
                        }
                    }
                }
            };
        }
    }
}
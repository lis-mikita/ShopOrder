#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Models.Orders;
using ShopOrder.Domain.Interfaces.Orders;
using ShopOrder.Infrastructure.Business.Orders;
using ShopOrder.Services.Interfaces.Orders;
using static ShopOrder.Infrastructure.UnitTests.Orders.TestData.OrdersTestData;
using OrderValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.Orders;
using ShopOrder.Domain.Core.Models.OrderDetails;

namespace ShopOrder.Infrastructure.UnitTests.Orders
{
    public class CreateOrderAsync
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly IOrderService _orderService;
        private readonly CreateOrderAsync_TestData TestData;

        private Order Order { get; set; }
        private ModelStateDictionary ModelState { get; set; }

        public CreateOrderAsync()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object);
            TestData = new CreateOrderAsync_TestData();

            Order = TestData.Orders.FirstOrDefault()!;
            ModelState = TestData.ModelState;
        }

        [Fact]
        public async Task Order_creation_with_null_should_return_BadRequest()
        {
            var actual = await _orderService.CreateOrderAsync(null, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(CommonValidationMessages.IncorrectDataProvided(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Order_creation_with_invalid_model_should_return_BadRequest()
        {
            const string errorKey = "ErrorKey";
            const string errorValue = "ErrorValue";
            ModelState.AddModelError(errorKey, errorValue);

            var actual = await _orderService.CreateOrderAsync(Order, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(errorValue, actual.Result.Error.Value);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
        }

        [Fact]
        public async Task Order_creation_without_existing_user_should_return_BadResponse()
        {
            _orderRepositoryMock.Setup(repo => repo.CheckUserExistsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            var actual = await _orderService.CreateOrderAsync(Order, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(OrderValidationMessages.IncorrectUserId(Order.UserId), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Order_creation_should_return_OrderResponse_with_Order()
        {
            _orderRepositoryMock.Setup(repo => repo.CheckUserExistsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            _orderRepositoryMock.Setup(repo => repo.CreateOrderAsync(Order))
                .Returns(Task.FromResult<Order?>(Order));

            var actual = await _orderService.CreateOrderAsync(Order, ModelState);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.Order);
        }

        [Theory]
        [InlineData(10, 5, 50)]
        [InlineData(0, 0, 0)]
        public async Task Order_creation_with_computed_total_amount_should_return_OrderResponse_with_Order(decimal price, int quantity, decimal totalAmount)
        {
            Order.OrderDetails = new[] {
                new OrderDetail
                {
                    OrderId = Order.OrderId,
                    ProductName = "test",
                    Price = price,
                    Quantity = quantity
                }
            };
            _orderRepositoryMock.Setup(repo => repo.CheckUserExistsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            _orderRepositoryMock.Setup(repo => repo.CreateOrderAsync(Order))
                .Returns(Task.FromResult<Order?>(Order));

            var actual = await _orderService.CreateOrderAsync(Order, ModelState);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.Order);
            Assert.Equal(totalAmount, actual.Order.TotalAmount);
        }

        [Fact]
        public async Task Order_creation_with_exception_should_return_InternalServerError()
        {
            _orderRepositoryMock.Setup(repo => repo.CheckUserExistsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            _orderRepositoryMock.Setup(repo => repo.CreateOrderAsync(Order))
                .Throws<Exception>();

            var actual = await _orderService.CreateOrderAsync(Order, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
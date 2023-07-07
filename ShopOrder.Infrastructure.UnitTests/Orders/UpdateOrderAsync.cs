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
    public class UpdateOrderAsync
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly IOrderService _orderService;
        private readonly UpdateOrderAsync_TestData TestData;

        private Order Order { get; set; }
        private int Id { get; set; }
        private ModelStateDictionary ModelState { get; set; }

        public UpdateOrderAsync()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object);
            TestData = new UpdateOrderAsync_TestData();

            Order = TestData.Orders.FirstOrDefault()!;
            ModelState = TestData.ModelState;
            Id = TestData.Id;
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Update_order_with_invalid_id_should_return_BadRequest(int id)
        {
            var actual = await _orderService.UpdateOrderAsync(id, Order, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(OrderValidationMessages.IncorrectId(id), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Update_order_with_null_should_return_BadRequest()
        {
            var actual = await _orderService.UpdateOrderAsync(Id, null, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(CommonValidationMessages.IncorrectDataProvided(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Update_order_with_invalid_model_should_return_BadRequest()
        {
            const string errorKey = "ErrorKey";
            const string errorValue = "ErrorValue";
            ModelState.AddModelError(errorKey, errorValue);

            var actual = await _orderService.UpdateOrderAsync(Id, Order, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(errorValue, actual.Result.Error.Value);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
        }

        [Theory]
        [InlineData(10, 5, 50)]
        [InlineData(0, 0, 0)]
        public async Task Updated_order_with_computed_total_amount_should_return_OrderResponse_with_Order(decimal price, int quantity, decimal totalAmount)
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
            _orderRepositoryMock.Setup(repo => repo.UpdateOrderAsync(Id, Order))
                .Returns(Task.FromResult<Order?>(Order));

            var actual = await _orderService.UpdateOrderAsync(Id, Order, ModelState);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.Order);
            Assert.Equal(totalAmount, actual.Order.TotalAmount);
        }

        [Fact]
        public async Task Updated_order_should_return_OrderResponse_with_Order()
        {
            _orderRepositoryMock.Setup(repo => repo.UpdateOrderAsync(It.IsAny<int>(), Order))
                .Returns(Task.FromResult<Order?>(Order));

            var actual = await _orderService.UpdateOrderAsync(Id, Order, ModelState);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.Order);
        }

        [Fact]
        public async Task Updated_order_not_should_return_NotFoundResponse()
        {
            _orderRepositoryMock.Setup(repo => repo.UpdateOrderAsync(It.IsAny<int>(), Order))
                .Returns(Task.FromResult<Order?>(null));

            var actual = await _orderService.UpdateOrderAsync(Id, Order, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(OrderValidationMessages.OrderNotFound(Order.OrderId), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Updating_order_with_exception_should_return_InternalServerError()
        {
            _orderRepositoryMock.Setup(repo => repo.UpdateOrderAsync(It.IsAny<int>(), Order))
                .Throws<Exception>();

            var actual = await _orderService.UpdateOrderAsync(Id, Order, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
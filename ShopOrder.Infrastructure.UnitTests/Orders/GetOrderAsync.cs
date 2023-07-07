using ShopOrder.Domain.Core.Models.Orders;
using ShopOrder.Domain.Interfaces.Orders;
using ShopOrder.Infrastructure.Business.Orders;
using ShopOrder.Services.Interfaces.Orders;
using static ShopOrder.Infrastructure.UnitTests.Orders.TestData.OrdersTestData;
using OrderValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.Orders;

namespace ShopOrder.Infrastructure.UnitTests.Orders
{
    public class GetOrderAsync
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly IOrderService _orderService;
        private readonly GetOrderAsync_TestData TestData;

        private Order Order { get; set; }

        public GetOrderAsync()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object);
            TestData = new GetOrderAsync_TestData();

            Order = TestData.Orders.FirstOrDefault()!;
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Order_with_invalid_id_should_return_BadRequest(int id)
        {
            var actual = await _orderService.GetOrderAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(OrderValidationMessages.IncorrectId(id), actual.Result.Error.Value);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(int.MaxValue)]
        public async Task Order_not_found_should_return_NotFoundResponse(int id)
        {
            _orderRepositoryMock.Setup(repo => repo.GetOrderAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Order?>(null));

            var actual = await _orderService.GetOrderAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(OrderValidationMessages.OrderNotFound(id), actual.Result.Error.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async Task Order_with_valid_id_should_return_OrderResponse_with_Order(int id)
        {
            _orderRepositoryMock.Setup(repo => repo.GetOrderAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(TestData.Orders.Find(u => u.OrderId == id)));

            var actual = await _orderService.GetOrderAsync(id);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.Order);
        }

        [Fact]
        public async Task Exception_while_getting_order_should_return_InternalServerError()
        {
            _orderRepositoryMock.Setup(repo => repo.GetOrderAsync(It.IsAny<int>()))
                .Throws<Exception>();

            var actual = await _orderService.GetOrderAsync(Order.OrderId);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
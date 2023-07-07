using ShopOrder.Domain.Core.Models.Orders;
using ShopOrder.Domain.Interfaces.Orders;
using ShopOrder.Infrastructure.Business.Orders;
using ShopOrder.Services.Interfaces.Orders;
using static ShopOrder.Infrastructure.UnitTests.Orders.TestData.OrdersTestData;
using OrderValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.Orders;

namespace ShopOrder.Infrastructure.UnitTests.Orders
{
    public class GetOrderWithOrderDetailsAsync
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly IOrderService _orderService;
        private readonly GetOrderWithOrderDetailsAsync_TestData TestData;

        private Order Order { get; set; }

        public GetOrderWithOrderDetailsAsync()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object);
            TestData = new GetOrderWithOrderDetailsAsync_TestData();

            Order = TestData.Orders.FirstOrDefault()!;
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Order_with_invalid_id_should_return_BadRequest(int id)
        {
            var actual = await _orderService.GetOrderWithOrderDetailsAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(OrderValidationMessages.IncorrectId(id), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Order_with_valid_id_should_return_OrderResponse_with_Order()
        {
            _orderRepositoryMock.Setup(repo => repo.GetOrderWithOrderDetailsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Order?>(Order));

            var actual = await _orderService.GetOrderWithOrderDetailsAsync(Order.OrderId);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.Order);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(int.MaxValue)]
        public async Task Order_not_found_should_return_NotFoundResponse(int id)
        {
            _orderRepositoryMock.Setup(repo => repo.GetOrderWithOrderDetailsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<Order?>(null));

            var actual = await _orderService.GetOrderWithOrderDetailsAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(OrderValidationMessages.OrderNotFound(id), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Exception_while_getting_order_should_return_InternalServerError()
        {
            _orderRepositoryMock.Setup(repo => repo.GetOrderWithOrderDetailsAsync(It.IsAny<int>()))
                .Throws<Exception>();

            var actual = await _orderService.GetOrderWithOrderDetailsAsync(Order.OrderId);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
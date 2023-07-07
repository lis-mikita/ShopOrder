using ShopOrder.Domain.Core.Models.Orders;
using ShopOrder.Domain.Interfaces.Orders;
using ShopOrder.Infrastructure.Business.Orders;
using ShopOrder.Services.Interfaces.Orders;
using static ShopOrder.Infrastructure.UnitTests.Orders.TestData.OrdersTestData;
using OrderValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.Orders;

namespace ShopOrder.Infrastructure.UnitTests.Orders
{
    public class GetOrdersAsync
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly IOrderService _orderService;
        private readonly GetOrdersAsync_TestData TestData;

        private IEnumerable<Order> Orders { get; set; }

        public GetOrdersAsync()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object);
            TestData = new GetOrdersAsync_TestData();

            Orders = TestData.Orders;
        }

        [Fact]
        public async Task Orders_not_found_should_return_NotFoundResponse()
        {
            _orderRepositoryMock.Setup(repo => repo.GetOrdersAsync())
                .Returns(Task.FromResult<IEnumerable<Order>?>(Enumerable.Empty<Order>()));

            var actual = await _orderService.GetOrdersAsync();

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(OrderValidationMessages.OrdersNotFound(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task GetOrdersAsync_with_existing_orders_should_return_list_of_orders()
        {
            _orderRepositoryMock.Setup(repo => repo.GetOrdersAsync())
                .Returns(Task.FromResult<IEnumerable<Order>?>(Orders));

            var actual = await _orderService.GetOrdersAsync();

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.Orders);
        }

        [Fact]
        public async Task Exception_while_getting_orders_should_return_InternalServerError()
        {
            _orderRepositoryMock.Setup(repo => repo.GetOrdersAsync())
                .Throws<Exception>();

            var actual = await _orderService.GetOrdersAsync();

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
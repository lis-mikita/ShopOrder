using ShopOrder.Domain.Interfaces.Orders;
using ShopOrder.Infrastructure.Business.Orders;
using ShopOrder.Services.Interfaces.Orders;
using static ShopOrder.Infrastructure.UnitTests.Orders.TestData.OrdersTestData;
using OrderValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.Orders;

namespace ShopOrder.Infrastructure.UnitTests.Orders
{
    public class DeleteOrderAsync
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly IOrderService _orderService;
        private readonly DeleteOrderAsync_TestData TestData;

        private int Id { get; set; }

        public DeleteOrderAsync()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderService = new OrderService(_orderRepositoryMock.Object);
            TestData = new DeleteOrderAsync_TestData();

            Id = TestData.Id;
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Delete_order_with_invalid_id_should_return_BadRequest(int id)
        {
            var actual = await _orderService.DeleteOrderAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(CommonValidationMessages.IncorrectDataProvided(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Delete_order_not_found_should_return_NotFoundResponse()
        {
            _orderRepositoryMock.Setup(repo => repo.DeleteOrderAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            var actual = await _orderService.DeleteOrderAsync(Id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(OrderValidationMessages.OrderNotFound(Id), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Delete_order_with_valid_id_should_return_OkResponse()
        {
            _orderRepositoryMock.Setup(repo => repo.DeleteOrderAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            var actual = await _orderService.DeleteOrderAsync(Id);

            Assert.True(actual.Result.Succeeded);
        }

        [Fact]
        public async Task Deleting_order_with_exception_should_return_InternalServerError()
        {
            _orderRepositoryMock.Setup(repo => repo.DeleteOrderAsync(It.IsAny<int>()))
                .Throws<Exception>();

            var actual = await _orderService.DeleteOrderAsync(Id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
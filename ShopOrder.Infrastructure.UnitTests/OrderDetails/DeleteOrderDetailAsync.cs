using ShopOrder.Domain.Interfaces.OrderDetails;
using ShopOrder.Infrastructure.Business.OrderDetails;
using ShopOrder.Services.Interfaces.OrderDetails;
using static ShopOrder.Infrastructure.UnitTests.OrderDetails.TestData.OrderDetailsTestData;
using OrderDetailValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.OrderDetails;

namespace ShopOrder.Infrastructure.UnitTests.OrderDetails
{
    public class DeleteOrderDetailAsync
    {
        private readonly Mock<IOrderDetailRepository> _orderDetailRepositoryMock;
        private readonly IOrderDetailService _orderDetailService;
        private readonly DeleteOrderDetailAsync_TestData TestData;

        private int Id { get; set; }

        public DeleteOrderDetailAsync()
        {
            _orderDetailRepositoryMock = new Mock<IOrderDetailRepository>();
            _orderDetailService = new OrderDetailService(_orderDetailRepositoryMock.Object);
            TestData = new DeleteOrderDetailAsync_TestData();

            Id = TestData.Id;
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Delete_orderDetail_with_invalid_id_should_return_BadRequest(int id)
        {
            var actual = await _orderDetailService.DeleteOrderDetailAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(CommonValidationMessages.IncorrectDataProvided(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Delete_orderDetail_not_found_should_return_NotFoundResponse()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.DeleteOrderDetailAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            var actual = await _orderDetailService.DeleteOrderDetailAsync(Id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(OrderDetailValidationMessages.OrderDetailNotFound(Id), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Delete_orderDetail_with_valid_id_should_return_OkResponse()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.DeleteOrderDetailAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            var actual = await _orderDetailService.DeleteOrderDetailAsync(Id);

            Assert.True(actual.Result.Succeeded);
        }

        [Fact]
        public async Task Deleting_orderDetail_with_exception_should_return_InternalServerError()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.DeleteOrderDetailAsync(It.IsAny<int>()))
                .Throws<Exception>();

            var actual = await _orderDetailService.DeleteOrderDetailAsync(Id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
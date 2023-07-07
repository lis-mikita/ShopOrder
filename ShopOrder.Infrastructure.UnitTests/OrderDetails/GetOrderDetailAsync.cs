using ShopOrder.Domain.Core.Models.OrderDetails;
using ShopOrder.Domain.Interfaces.OrderDetails;
using ShopOrder.Infrastructure.Business.OrderDetails;
using ShopOrder.Services.Interfaces.OrderDetails;
using static ShopOrder.Infrastructure.UnitTests.OrderDetails.TestData.OrderDetailsTestData;
using OrderDetailValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.OrderDetails;

namespace ShopOrder.Infrastructure.UnitTests.OrderDetails
{
    public class GetOrderDetailAsync
    {
        private readonly Mock<IOrderDetailRepository> _orderDetailRepositoryMock;
        private readonly IOrderDetailService _orderDetailService;
        private readonly GetOrderDetailAsync_TestData TestData;

        private IEnumerable<OrderDetail> OrderDetails { get; set; }
        private OrderDetail OrderDetail { get; set; }

        public GetOrderDetailAsync()
        {
            _orderDetailRepositoryMock = new Mock<IOrderDetailRepository>();
            _orderDetailService = new OrderDetailService(_orderDetailRepositoryMock.Object);
            TestData = new GetOrderDetailAsync_TestData();

            OrderDetails = TestData.OrderDetails;
            OrderDetail = TestData.OrderDetails.FirstOrDefault()!;
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task OrderDetail_with_invalid_id_should_return_BadRequest(int id)
        {
            var actual = await _orderDetailService.GetOrderDetailAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(OrderDetailValidationMessages.IncorrectId(id), actual.Result.Error.Value);
        }

        [Theory]
        [InlineData(10)]
        [InlineData(int.MaxValue)]
        public async Task OrderDetail_not_found_should_return_NotFoundResponse(int id)
        {
            _orderDetailRepositoryMock.Setup(repo => repo.GetOrderDetailAsync(It.IsAny<int>()))
                .Returns(Task.FromResult<OrderDetail?>(null));

            var actual = await _orderDetailService.GetOrderDetailAsync(id);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(OrderDetailValidationMessages.OrderDetailNotFound(id), actual.Result.Error.Value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(3)]
        public async Task OrderDetail_with_valid_id_should_return_OrderDetailResponse_with_OrderDetail(int id)
        {
            _orderDetailRepositoryMock.Setup(repo => repo.GetOrderDetailAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(OrderDetails.FirstOrDefault(od => od.OrderDetailId == id)));

            var actual = await _orderDetailService.GetOrderDetailAsync(id);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.OrderDetail);
        }

        [Fact]
        public async Task Exception_while_getting_orderDetail_should_return_InternalServerError()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.GetOrderDetailAsync(It.IsAny<int>()))
                .Throws<Exception>();

            var actual = await _orderDetailService.GetOrderDetailAsync(OrderDetail.OrderDetailId);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
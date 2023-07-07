using OrderDetailValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.OrderDetails;
using ShopOrder.Domain.Core.Models.OrderDetails;
using ShopOrder.Domain.Interfaces.OrderDetails;
using ShopOrder.Infrastructure.Business.OrderDetails;
using ShopOrder.Services.Interfaces.OrderDetails;
using static ShopOrder.Infrastructure.UnitTests.OrderDetails.TestData.OrderDetailsTestData;

namespace ShopOrder.Infrastructure.UnitTests.OrderDetails
{
    public class GetOrderDetailsAsync
    {
        private readonly Mock<IOrderDetailRepository> _orderDetailRepositoryMock;
        private readonly IOrderDetailService _orderDetailService;
        private readonly GetOrderDetailsAsync_TestData TestData;

        private IEnumerable<OrderDetail> OrderDetails { get; set; }

        public GetOrderDetailsAsync()
        {
            _orderDetailRepositoryMock = new Mock<IOrderDetailRepository>();
            _orderDetailService = new OrderDetailService(_orderDetailRepositoryMock.Object);
            TestData = new GetOrderDetailsAsync_TestData();

            OrderDetails = TestData.OrderDetails;
        }

        [Fact]
        public async Task Order_details_not_found_should_return_NotFoundResponse()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.GetOrderDetailsAsync())
                .Returns(Task.FromResult(Enumerable.Empty<OrderDetail>()));

            var actual = await _orderDetailService.GetOrderDetailsAsync();

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(OrderDetailValidationMessages.OrderDetailsNotFound(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task GetOrderDetailsAsync_with_existing_order_details_should_return_list_of_orderDetails()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.GetOrderDetailsAsync())
                .Returns(Task.FromResult(OrderDetails));

            var actual = await _orderDetailService.GetOrderDetailsAsync();

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.OrderDetails);
        }

        [Fact]
        public async Task Exception_while_getting_order_details_should_return_InternalServerError()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.GetOrderDetailsAsync())
                .Throws<Exception>();

            var actual = await _orderDetailService.GetOrderDetailsAsync();

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
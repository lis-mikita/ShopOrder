#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Models.OrderDetails;
using ShopOrder.Domain.Interfaces.OrderDetails;
using ShopOrder.Infrastructure.Business.OrderDetails;
using ShopOrder.Services.Interfaces.OrderDetails;
using static ShopOrder.Infrastructure.UnitTests.OrderDetails.TestData.OrderDetailsTestData;
using OrderDetailValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.OrderDetails;

namespace ShopOrder.Infrastructure.UnitTests.OrderDetails
{
    public class CreateOrderDetailAsync
    {
        private readonly Mock<IOrderDetailRepository> _orderDetailRepositoryMock;
        private readonly IOrderDetailService _orderDetailService;
        private readonly CreateOrderDetailAsync_TestData TestData;

        private OrderDetail OrderDetail { get; set; }
        private ModelStateDictionary ModelState { get; set; }

        public CreateOrderDetailAsync()
        {
            _orderDetailRepositoryMock = new Mock<IOrderDetailRepository>();
            _orderDetailService = new OrderDetailService(_orderDetailRepositoryMock.Object);
            TestData = new CreateOrderDetailAsync_TestData();

            OrderDetail = TestData.OrderDetails.FirstOrDefault()!;
            ModelState = TestData.ModelState;
        }

        [Fact]
        public async Task Order_detail_creation_with_null_should_return_BadRequest()
        {
            var actual = await _orderDetailService.CreateOrderDetailAsync(null, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(CommonValidationMessages.IncorrectDataProvided(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Order_detail_creation_with_invalid_model_should_return_BadRequest()
        {
            const string errorKey = "ErrorKey";
            const string errorValue = "ErrorValue";
            ModelState.AddModelError(errorKey, errorValue);

            var actual = await _orderDetailService.CreateOrderDetailAsync(OrderDetail, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(errorValue, actual.Result.Error.Value);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
        }

        [Fact]
        public async Task Order_detail_creation_without_existing_order_should_return_BadResponse()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.CheckOrderExistsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            var actual = await _orderDetailService.CreateOrderDetailAsync(OrderDetail, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(OrderDetailValidationMessages.IncorrectOrderId(OrderDetail.OrderId), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Order_detail_creation_should_return_OrderDetailResponse_with_OrderDetail()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.CheckOrderExistsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            _orderDetailRepositoryMock.Setup(repo => repo.CreateOrderDetailAsync(OrderDetail))
                .Returns(Task.FromResult<OrderDetail?>(OrderDetail));

            var actual = await _orderDetailService.CreateOrderDetailAsync(OrderDetail, ModelState);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.OrderDetail);
        }

        [Fact]
        public async Task Order_detail_creation_with_exception_should_return_InternalServerError()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.CheckOrderExistsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(true));
            _orderDetailRepositoryMock.Setup(repo => repo.CreateOrderDetailAsync(OrderDetail))
                .Throws<Exception>();

            var actual = await _orderDetailService.CreateOrderDetailAsync(OrderDetail, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
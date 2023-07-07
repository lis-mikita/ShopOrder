#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

using OrderDetailValidationMessages = ShopOrder.Domain.Core.Infrastructure.Constants.Validation.OrderDetails;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Models.OrderDetails;
using ShopOrder.Domain.Interfaces.OrderDetails;
using ShopOrder.Infrastructure.Business.OrderDetails;
using ShopOrder.Services.Interfaces.OrderDetails;
using static ShopOrder.Infrastructure.UnitTests.OrderDetails.TestData.OrderDetailsTestData;

namespace ShopOrder.Infrastructure.UnitTests.OrderDetails
{
    public class UpdateOrderDetailAsync
    {
        private readonly Mock<IOrderDetailRepository> _orderDetailRepositoryMock;
        private readonly IOrderDetailService _orderDetailService;
        private readonly UpdateOrderDetailAsync_TestData TestData;

        private OrderDetail OrderDetail { get; set; }
        private ModelStateDictionary ModelState { get; set; }
        private int Id { get; set; }

        public UpdateOrderDetailAsync()
        {
            _orderDetailRepositoryMock = new Mock<IOrderDetailRepository>();
            _orderDetailService = new OrderDetailService(_orderDetailRepositoryMock.Object);
            TestData = new UpdateOrderDetailAsync_TestData();

            OrderDetail = TestData.OrderDetails.FirstOrDefault()!;
            ModelState = TestData.ModelState;
            Id = TestData.Id;
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task Update_order_detail_with_invalid_id_should_return_BadRequest(int id)
        {
            var actual = await _orderDetailService.UpdateOrderDetailAsync(id, OrderDetail, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(OrderDetailValidationMessages.IncorrectId(id), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Update_order_detail_with_null_should_return_BadRequest()
        {
            var actual = await _orderDetailService.UpdateOrderDetailAsync(Id, null, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
            Assert.Equal(CommonValidationMessages.IncorrectDataProvided(), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Update_order_detail_with_invalid_model_should_return_BadRequest()
        {
            const string errorKey = "ErrorKey";
            const string errorValue = "ErrorValue";
            ModelState.AddModelError(errorKey, errorValue);

            var actual = await _orderDetailService.UpdateOrderDetailAsync(Id, OrderDetail, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(errorValue, actual.Result.Error.Value);
            Assert.Equal(HttpStatusCode.BadRequest, actual.Result.Error.Key);
        }

        [Fact]
        public async Task Updated_order_detail_should_return_OrderDetailResponse_with_OrderDetail()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.UpdateOrderDetailAsync(It.IsAny<int>(), OrderDetail))
                .Returns(Task.FromResult<OrderDetail?>(OrderDetail));

            var actual = await _orderDetailService.UpdateOrderDetailAsync(Id, OrderDetail, ModelState);

            Assert.True(actual.Result.Succeeded);
            Assert.NotNull(actual.OrderDetail);
        }

        [Fact]
        public async Task Updated_order_detail_not_should_return_NotFoundResponse()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.UpdateOrderDetailAsync(It.IsAny<int>(), OrderDetail))
                .Returns(Task.FromResult<OrderDetail?>(null));

            var actual = await _orderDetailService.UpdateOrderDetailAsync(Id, OrderDetail, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.NotFound, actual.Result.Error.Key);
            Assert.Equal(OrderDetailValidationMessages.OrderDetailNotFound(OrderDetail.OrderDetailId), actual.Result.Error.Value);
        }

        [Fact]
        public async Task Updating_order_detail_with_exception_should_return_InternalServerError()
        {
            _orderDetailRepositoryMock.Setup(repo => repo.UpdateOrderDetailAsync(It.IsAny<int>(), OrderDetail))
                .Throws<Exception>();

            var actual = await _orderDetailService.UpdateOrderDetailAsync(Id, OrderDetail, ModelState);

            Assert.True(actual.Result.Failed);
            Assert.Equal(HttpStatusCode.InternalServerError, actual.Result.Error.Key);
        }
    }
}
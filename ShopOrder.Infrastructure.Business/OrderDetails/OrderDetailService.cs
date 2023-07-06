using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using ShopOrder.Domain.Core.Messages;
using ShopOrder.Domain.Core.Messages.OrderDetails;
using ShopOrder.Domain.Core.Models.OrderDetails;
using ShopOrder.Domain.Interfaces.OrderDetails;
using ShopOrder.Services.Interfaces.OrderDetails;
using System.Data.SqlClient;
using System.Net;
using static ShopOrder.Domain.Core.Infrastructure.Constants;

namespace ShopOrder.Infrastructure.Business.OrderDetails
{
    public class OrderDetailService : IOrderDetailService
    {
        private const int MIN_ORDER_DETAIL_ID = 1;
        private readonly IOrderDetailRepository _orderDetailRepository;

        public OrderDetailService(IOrderDetailRepository orderDetailRepository) => _orderDetailRepository = orderDetailRepository;

        public async Task<OrderDetailResponse> CreateOrderDetailAsync(OrderDetail orderDetail, ModelStateDictionary modelState)
        {
            try
            {
                if (orderDetail is null)
                {
                    return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.BadRequest,
                        Validation.CommonErrors.IncorrectDataProvided());
                }

                if (!modelState.IsValid)
                {
                    var errorMessages = string.Join("; ", modelState.Values
                        .SelectMany(entry => entry.Errors)
                    .Select(error => error.ErrorMessage));

                    return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.BadRequest, errorMessages);
                }

                if (!await _orderDetailRepository.CheckOrderExistsAsync(orderDetail.OrderId))
                {
                    return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.BadRequest,
                        Validation.OrderDetails.IncorrectOrderId(orderDetail.OrderId));
                }

                var orderDetailNew = await _orderDetailRepository.CreateOrderDetailAsync(orderDetail);

                return new OrderDetailResponse { OrderDetail = orderDetailNew };
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SavingError(ex.Message));
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<BaseResponse> DeleteOrderDetailAsync(int id)
        {
            try
            {
                if (id < MIN_ORDER_DETAIL_ID)
                {
                    return BaseResponse.Failure(HttpStatusCode.BadRequest,
                        Validation.CommonErrors.IncorrectDataProvided());
                }

                var deleted = await _orderDetailRepository.DeleteOrderDetailAsync(id);
                if (!deleted)
                {
                    return BaseResponse.Failure(HttpStatusCode.NotFound,
                        Validation.OrderDetails.OrderDetailNotFound(id));
                }

                return BaseResponse.Success;
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SavingError(ex.Message));
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<OrderDetailResponse> GetOrderDetailAsync(int id)
        {
            try
            {
                if (id < MIN_ORDER_DETAIL_ID)
                {
                    return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.BadRequest,
                        Validation.OrderDetails.IncorrectId(id));
                }

                var orderDetail = await _orderDetailRepository.GetOrderDetailAsync(id);
                if (orderDetail is null)
                {
                    return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.NotFound,
                        Validation.OrderDetails.OrderDetailNotFound(id));
                }

                return new OrderDetailResponse() { OrderDetail = orderDetail };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<OrderDetailsResponse> GetOrderDetailsAsync()
        {
            try
            {
                var orderDetails = await _orderDetailRepository.GetOrderDetailsAsync();
                if (orderDetails is null || !orderDetails.Any())
                {
                    return BaseResponse.Failure<OrderDetailsResponse>(HttpStatusCode.NotFound,
                        Validation.OrderDetails.OrderDetailsNotFound());
                }

                return new OrderDetailsResponse { OrderDetails = orderDetails };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<OrderDetailsResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<OrderDetailsResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<OrderDetailResponse> UpdateOrderDetailAsync(int id, OrderDetail orderDetail, ModelStateDictionary modelState)
        {
            try
            {
                if (id < MIN_ORDER_DETAIL_ID)
                {
                    return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.BadRequest,
                        Validation.OrderDetails.IncorrectId(id));
                }

                if (orderDetail is null)
                {
                    return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.BadRequest,
                        Validation.CommonErrors.IncorrectDataProvided());
                }

                if (!modelState.IsValid)
                {
                    var errorMessages = string.Join("; ", modelState.Values
                        .SelectMany(entry => entry.Errors)
                        .Select(error => error.ErrorMessage));

                    return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.BadRequest, errorMessages);
                }

                var updatedOrderDetail = await _orderDetailRepository.UpdateOrderDetailAsync(id, orderDetail);
                if (updatedOrderDetail is null)
                {
                    return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.NotFound,
                        Validation.OrderDetails.OrderDetailNotFound(orderDetail.OrderDetailId));
                }

                return new OrderDetailResponse { OrderDetail = updatedOrderDetail };
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SavingError(ex.Message));
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<OrderDetailResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }
    }
}
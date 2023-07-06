using Microsoft.AspNetCore.Mvc.ModelBinding;
using ShopOrder.Domain.Core.Messages;
using System.Data.SqlClient;
using System.Net;
using ShopOrder.Domain.Interfaces.Orders;
using ShopOrder.Services.Interfaces.Orders;
using ShopOrder.Domain.Core.Messages.Orders;
using ShopOrder.Domain.Core.Models.Orders;
using static ShopOrder.Domain.Core.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;

namespace ShopOrder.Infrastructure.Business.Orders
{
    public class OrderService : IOrderService
    {
        private const int MIN_ORDER_ID = 1;
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository) => _orderRepository = orderRepository;

        public async Task<OrderResponse> CreateOrderAsync(Order order, ModelStateDictionary modelState)
        {
            try
            {
                if (order is null)
                {
                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.BadRequest,
                        Validation.CommonErrors.IncorrectDataProvided());
                }

                if (!modelState.IsValid)
                {
                    var errorMessages = string.Join("; ", modelState.Values
                        .SelectMany(entry => entry.Errors)
                        .Select(error => error.ErrorMessage));

                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.BadRequest, errorMessages);
                }

                if (!await _orderRepository.CheckUserExistsAsync(order.UserId))
                {
                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.BadRequest,
                        Validation.Orders.IncorrectUserId(order.UserId));
                }

                order.OrderDate = DateTime.Now;
                if (order.OrderDetails is not null)
                {
                    decimal totalAmount = 0;
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        totalAmount += orderDetail.Price * orderDetail.Quantity;
                    }

                    order.TotalAmount = totalAmount;
                }

                var orderNew = await _orderRepository.CreateOrderAsync(order);

                return new OrderResponse { Order = orderNew };
            }
            catch (DbUpdateException ex)
            {
                return BaseResponse.Failure<OrderResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SavingError(ex.Message));
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<OrderResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<OrderResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<BaseResponse> DeleteOrderAsync(int id)
        {
            try
            {
                if (id < MIN_ORDER_ID)
                {
                    return BaseResponse.Failure(HttpStatusCode.BadRequest,
                        Validation.CommonErrors.IncorrectDataProvided());
                }

                var deleted = await _orderRepository.DeleteOrderAsync(id);

                if (!deleted)
                {
                    return BaseResponse.Failure(HttpStatusCode.NotFound,
                        Validation.Orders.OrderNotFound(id));
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

        public async Task<OrderResponse> GetOrderAsync(int id)
        {
            try
            {
                if (id < MIN_ORDER_ID)
                {
                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.BadRequest,
                        Validation.Orders.IncorrectId(id));
                }

                var order = await _orderRepository.GetOrderAsync(id);
                if (order is null)
                {
                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.NotFound,
                        Validation.Orders.OrderNotFound(id));
                }

                return new OrderResponse() { Order = order };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<OrderResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<OrderResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<OrdersResponse> GetOrdersAsync()
        {
            try
            {
                var orders = await _orderRepository.GetOrdersAsync();

                if (orders is null || !orders.Any())
                {
                    return BaseResponse.Failure<OrdersResponse>(HttpStatusCode.NotFound,
                        Validation.Orders.OrdersNotFound());
                }

                return new OrdersResponse { Orders = orders };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<OrdersResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<OrdersResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<OrderResponse> GetOrderWithOrderDetailsAsync(int id)
        {
            try
            {
                if (id < MIN_ORDER_ID)
                {
                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.BadRequest,
                        Validation.Orders.IncorrectId(id));
                }

                var order = await _orderRepository.GetOrderWithOrderDetailsAsync(id);
                if (order is null)
                {
                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.NotFound,
                        Validation.Orders.OrderNotFound(id));
                }

                return new OrderResponse() { Order = order };
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<OrderResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<OrderResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

        public async Task<OrderResponse> UpdateOrderAsync(int id, Order order, ModelStateDictionary modelState)
        {
            try
            {
                if (id < MIN_ORDER_ID)
                {
                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.BadRequest,
                        Validation.Orders.IncorrectId(id));
                }

                if (order is null)
                {
                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.BadRequest,
                        Validation.CommonErrors.IncorrectDataProvided());
                }

                if (!modelState.IsValid)
                {
                    var errorMessages = string.Join("; ", modelState.Values
                        .SelectMany(entry => entry.Errors)
                        .Select(error => error.ErrorMessage));

                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.BadRequest, errorMessages);
                }

                if (order.OrderDetails is not null)
                {
                    decimal totalAmount = 0;
                    foreach (var orderDetail in order.OrderDetails)
                    {
                        totalAmount += orderDetail.Price * orderDetail.Quantity;
                    }

                    order.TotalAmount = totalAmount;
                }

                var updatedOrder = await _orderRepository.UpdateOrderAsync(id, order);

                if (updatedOrder is null)
                {
                    return BaseResponse.Failure<OrderResponse>(HttpStatusCode.NotFound,
                        Validation.Orders.OrderNotFound(order.OrderId));
                }

                return new OrderResponse { Order = updatedOrder };

            }
            catch (DbUpdateException ex)
            {
                return BaseResponse.Failure<OrderResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SavingError(ex.Message));
            }
            catch (SqlException ex)
            {
                return BaseResponse.Failure<OrderResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.SQLError(ex.Message));
            }
            catch (Exception ex)
            {
                return BaseResponse.Failure<OrderResponse>(HttpStatusCode.InternalServerError,
                    Validation.CommonErrors.ServerError(ex.Message));
            }
        }

    }
}
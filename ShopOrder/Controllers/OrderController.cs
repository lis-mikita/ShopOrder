using Microsoft.AspNetCore.Mvc;
using ShopOrder.Domain.Core.Models.Orders;
using ShopOrder.Services.Interfaces.Orders;

namespace ShopOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            var response = await _orderService.GetOrderAsync(id);
            if (response.Result.Succeeded)
            {
                return Ok(response.Order);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("GetOrder request failed. Order ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpGet]
        [Route("get-with-orderdetails/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderWithOrdersAsync(int id)
        {
            var response = await _orderService.GetOrderWithOrderDetailsAsync(id);
            if (response.Result.Succeeded)
            {
                return Ok(response.Order);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("GetOrderWithOrderDetails request failed. Order ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Order>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            var response = await _orderService.GetOrdersAsync();
            if (response.Result.Succeeded)
            {
                return Ok(response.Orders);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("GetOrders request failed." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                response.Result.Error, request.Method, request.Path);


            return StatusCode(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var response = await _orderService.CreateOrderAsync(order, ModelState);
            if (response.Result.Succeeded)
            {
                return Ok(response.Order);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("CreateOrder request failed." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                response.Result.Error, request.Method, request.Path);


            return StatusCode(response);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var response = await _orderService.DeleteOrderAsync(id);
            if (response.Result.Succeeded)
            {
                return Ok();
            }

            var request = HttpContext.Request;
            _logger.LogWarning("DeleteOrder request failed. Order ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);


            return StatusCode(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Order))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            var response = await _orderService.UpdateOrderAsync(id, order, ModelState);
            if (response.Result.Succeeded)
            {
                return Ok(response.Order);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("UpdateOrder request failed. Order ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);


            return StatusCode(response);
        }
    }
}
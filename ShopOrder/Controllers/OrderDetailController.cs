using Microsoft.AspNetCore.Mvc;
using ShopOrder.Domain.Core.Models.OrderDetails;
using ShopOrder.Services.Interfaces.OrderDetails;

namespace ShopOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class OrderDetailController : BaseController
    {
        private readonly IOrderDetailService _orderDetailService;
        private readonly ILogger<OrderDetailController> _logger;

        public OrderDetailController(IOrderDetailService orderDetailService, ILogger<OrderDetailController> logger)
        {
            _orderDetailService = orderDetailService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderDetailAsync(int id)
        {
            var response = await _orderDetailService.GetOrderDetailAsync(id);
            if (response.Result.Succeeded)
            {
                return Ok(response.OrderDetail);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("GetOrderDetail request failed. OrderDetail ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDetail>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllOrderDetailsAsync()
        {
            var response = await _orderDetailService.GetOrderDetailsAsync();
            if (response.Result.Succeeded)
            {
                return Ok(response.OrderDetails);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("GetOrderDetails request failed." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrderDetail(OrderDetail orderDetail)
        {
            var response = await _orderDetailService.CreateOrderDetailAsync(orderDetail, ModelState);
            if (response.Result.Succeeded)
            {
                return Ok(response.OrderDetail);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("CreateOrderDetail request failed." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            var response = await _orderDetailService.DeleteOrderDetailAsync(id);
            if (response.Result.Succeeded)
            {
                return Ok();
            }

            var request = HttpContext.Request;
            _logger.LogWarning("DeleteOrderDetail request failed. OrderDetail ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDetail))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateOrderDetail(int id, OrderDetail orderDetail)
        {
            var response = await _orderDetailService.UpdateOrderDetailAsync(id, orderDetail, ModelState);
            if (response.Result.Succeeded)
            {
                return Ok(response.OrderDetail);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("UpdateOrderDetail request failed. OrderDetail ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }
    }
}
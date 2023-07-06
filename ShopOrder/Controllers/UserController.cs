using Microsoft.AspNetCore.Mvc;
using ShopOrder.Domain.Core.Models.Users;
using ShopOrder.Services.Interfaces.Users;

namespace ShopOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserAsync(int id)
        {
            var response = await _userService.GetUserAsync(id);
            if (response.Result.Succeeded)
            {
                return Ok(response.User);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("GetUser request failed. User ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpGet]
        [Route("get-with-orders/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserWithOrdersAsync(int id)
        {
            var response = await _userService.GetUserWithOrdersAsync(id);
            if (response.Result.Succeeded)
            {
                return Ok(response.User);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("GetUserWithOrders request failed. User ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpGet]
        [Route("get-all")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var response = await _userService.GetUsersAsync();
            if (response.Result.Succeeded)
            {
                return Ok(response.Users);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("GetUsers request failed." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser(User user)
        {
            var response = await _userService.CreateUserAsync(user, ModelState);
            if (response.Result.Succeeded)
            {
                return Ok(response.User);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("CreateUser request failed." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await _userService.DeleteUserAsync(id);
            if (response.Result.Succeeded)
            {
                return Ok();
            }

            var request = HttpContext.Request;
            _logger.LogWarning("DeleteUser request failed. User ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            var response = await _userService.UpdateUserAsync(id, user, ModelState);
            if (response.Result.Succeeded)
            {
                return Ok(response.User);
            }

            var request = HttpContext.Request;
            _logger.LogWarning("UpdateUser request failed. User ID: {Id}." +
                " Error: {ErrorMessage}. Request: {RequestMethod} {RequestPath}",
                id, response.Result.Error, request.Method, request.Path);

            return StatusCode(response);
        }
    }
}
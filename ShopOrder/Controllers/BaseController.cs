using Microsoft.AspNetCore.Mvc;
using ShopOrder.Domain.Core.Messages;

namespace ShopOrder.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected ObjectResult StatusCode(BaseResponse response) =>
            StatusCode((int)response.Result.Error.Key, response.Result.Error.Value);
    }
}
using System.Net;

namespace ShopOrder.Domain.Core.Messages
{
    public class BaseResponse
    {
        public ResponseResult Result { get; set; } = ResponseResult.Success;

        public static T Failure<T>(HttpStatusCode code, string error) where T : BaseResponse, new() =>
            new()
            {
                Result = ResponseResult.Failure(code, error)
            };

        public static BaseResponse Failure(HttpStatusCode code, string error) =>
            new()
            {
                Result = ResponseResult.Failure(code, error)
            };

        public static BaseResponse Success => new();
    }
}
using System.Net;

namespace ShopOrder.Domain.Core.Messages
{
    public class ResponseResult
    {
        public ResponseResult(bool succeeded)
        {
            Succeeded = succeeded;
        }

        public bool Succeeded { get; private set; }

        public bool Failed
        {
            get { return !Succeeded; }
        }

        public KeyValuePair<HttpStatusCode, string> Error { get; private set; }

        public static ResponseResult Success
        {
            get { return new ResponseResult(true); }
        }

        public static ResponseResult Failure(HttpStatusCode code, string error)
        {
            var failure = new ResponseResult(false);

            if (!string.IsNullOrWhiteSpace(error))
            {
                failure.Error = new KeyValuePair<HttpStatusCode, string>(code, error);
            }

            return failure;
        }
    }
}
using System.Net;

namespace JG.Infrastructure.Exceptions
{
    public class ApiException : AppException
    {
        public HttpStatusCode StatusCode { get; set; }

        public ApiException(HttpStatusCode statusCode, int errorCodeId, string developerMessage)
        {
            StatusCode = statusCode;
            ErrorCodeId = errorCodeId;
            DeveloperMessage = developerMessage;
        }
    }
}

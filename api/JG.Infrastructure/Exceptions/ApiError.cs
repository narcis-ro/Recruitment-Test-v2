using System.Net;

namespace JG.Infrastructure.Exceptions
{
    public class ApiError
    {
        public int ErrorCode { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string DeveloperMessage { get; set; }

        public object Details { get; set; }
    }
}

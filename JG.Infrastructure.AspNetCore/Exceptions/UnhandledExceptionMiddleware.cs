using System;
using System.Net;
using System.Threading.Tasks;
using JG.Infrastructure.Constants;
using JG.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;

namespace JG.Infrastructure.AspNetCore.Exceptions
{
    public class UnhandledExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public UnhandledExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            // Note: In case middleware throws an error, the MVC filter will not have a chance to handle this.
            // Note: At this point we only return the ApiException.Status code and set the response reason header, because we don't know the accept-type xml/json
            // Note: and we don't want to serialize the exception ourselves.
            catch (ApiException apiException)
            {
                httpContext.Response.StatusCode = (int) apiException.StatusCode;
                httpContext.Response.Headers.Add(HttpHeaders.REASON_PHRASE,
                    apiException.DeveloperMessage ?? apiException.Message);
            }
            catch (Exception)
            {
                // Note: There is no need to log anything here. RequestLoggingMiddleware will handle logging.
                httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            }
        }
    }
}

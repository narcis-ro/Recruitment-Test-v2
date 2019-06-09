using System.Linq;
using System.Net;
using FluentValidation;
using JG.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JG.Infrastructure.AspNetCore.Filters
{
    // ReSharper disable once UnusedMember.Global
    public class DefaultDomainExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            int errorCode;
            string message;
            object details;

            switch (context.Exception)
            {
                case ApiException apiException:
                    message = apiException.DeveloperMessage ?? apiException.Message;
                    statusCode = apiException.StatusCode;
                    errorCode = apiException.ErrorCodeId;
                    details = apiException.Data;

                    break;
                case DomainException domainException:
                    errorCode = domainException.ErrorCodeId;
                    message = domainException.DeveloperMessage ?? domainException.Message;
                    details = domainException.Data;

                    statusCode = MapErrorCode(context, domainException) ?? statusCode;

                    break;
                // Note: Fluent Validation exceptions
                case ValidationException validationException:

                    var errors = validationException.Errors.ToList();

                    if (errors.Count == 1)
                        int.TryParse(errors.First().ErrorCode, out errorCode);
                    else
                        errorCode = (int) KnownErrorCodes.MultiValidationException;

                    statusCode = HttpStatusCode.BadRequest;
                    message = validationException.Message;
                    details = errors;

                    break;
                default:
                    // Note: Let global unhandled exception middleware handle this.
                    return;
            }
            
            var contextResult = new ObjectResult(new ApiError
            {
                StatusCode = statusCode,
                ErrorCode = errorCode,
                DeveloperMessage = message,
                Details = details
            })
            {
                StatusCode = (int) statusCode
            };

            context.Result = HandleException(context, contextResult) ?? contextResult;
        }

        protected virtual IActionResult HandleException(ExceptionContext context, ObjectResult currentResult)
        {
            // Note: Override this in each service to do custom errorCode to http status code mapping;

            return default;
        }

        protected virtual HttpStatusCode? MapErrorCode(ExceptionContext context, DomainException domainException)
        {
            return default;
        }
    }
}
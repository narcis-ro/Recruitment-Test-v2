using System.Net;
using JG.Infrastructure.AspNetCore.Filters;
using JG.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JG.FinTechTest.Filters
{
    // ReSharper disable once UnusedMember.Global
    /// <inheritdoc />
    public class DomainExceptionFilter : DefaultDomainExceptionFilter
    {
        // ReSharper disable once RedundantOverriddenMember - For demo purposes
        /// <inheritdoc />
        protected override HttpStatusCode? MapErrorCode(ExceptionContext context, DomainException domainException)
        {
            // TODO: Map Specific Domain error codes to HttpStatus

            return base.MapErrorCode(context, domainException);
        }
    }
}

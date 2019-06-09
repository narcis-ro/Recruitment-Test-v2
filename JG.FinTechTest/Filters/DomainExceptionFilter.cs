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
        /// <inheritdoc />
        protected override HttpStatusCode? MapErrorCode(ExceptionContext context, DomainException domainException)
        {
            return base.MapErrorCode(context, domainException);
        }
    }
}

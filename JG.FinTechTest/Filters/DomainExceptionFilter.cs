using System.Net;
using JG.Infrastructure.AspNetCore.Filters;
using JG.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JG.FinTechTest.Filters
{
    // ReSharper disable once UnusedMember.Global
    public class DomainExceptionFilter : DefaultDomainExceptionFilter
    {
        protected override HttpStatusCode? MapErrorCode(ExceptionContext context, DomainException domainException)
        {
            return base.MapErrorCode(context, domainException);
        }
    }
}
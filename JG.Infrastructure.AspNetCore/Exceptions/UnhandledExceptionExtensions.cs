using Microsoft.AspNetCore.Builder;

namespace JG.Infrastructure.AspNetCore.Exceptions
{
    public static class UnhandledExceptionExtensions
    { 
        public static IApplicationBuilder UseSafeExceptions(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<UnhandledExceptionMiddleware>();
            return builder;
        }
    }
}

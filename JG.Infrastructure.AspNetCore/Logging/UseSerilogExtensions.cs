using System;
using JG.Infrastructure.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace JG.Infrastructure.AspNetCore.Logging
{
    public static class UseSerilogExtensions
    {
        public static IWebHostBuilder UseSerilog(this IWebHostBuilder hostBuilder, Action<WebHostBuilderContext, ILoggingBuilder, LoggerConfiguration> configureLogging = default)
        {
            hostBuilder.ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                var serilogBuilder = new SerilogBuilder(hostingContext.Configuration)
                    .WithEnvironment(hostingContext.HostingEnvironment.EnvironmentName)
                    .WithConfig(configuration =>
                    {
                        configureLogging?.Invoke(hostingContext, loggingBuilder, configuration);
                        return configuration;
                    });

                Log.Logger = serilogBuilder.Build();
                GlobalLog.SetSerilog(Log.Logger);

                loggingBuilder.Services.AddSingleton(Log.Logger);
            });

            return hostBuilder;
        }

        /// <summary>
        /// Adds the request logging middleware to the application pipeline.
        /// </summary>
        /// <param name="builder">An application builder.</param>
        /// <param name="configureOptions">An optional action that can be used to configure the middleware options.</param>
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder, Action<RequestLoggingOptions> configureOptions = null)
        {
            var options = new RequestLoggingOptions();
            configureOptions?.Invoke(options);
            return builder.UseMiddleware<RequestLoggingMiddleware>(options);
        }
    }
}

using System;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using JG.FinTechTest.Api.Filters;
using JG.FinTechTest.Api.StartupTasks;
using JG.Infrastructure.AspNetCore.Correlation;
using JG.Infrastructure.AspNetCore.Exceptions;
using JG.Infrastructure.AspNetCore.Logging;
using JG.Infrastructure.AspNetCore.StartupTasks;
using JG.Infrastructure.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace JG.FinTechTest.Api
{
    /// <summary>
    /// AspNetCore startup
    /// </summary>
    public class Startup
    {
        private const string DefaultCorsPolicy = "Default";
        /// <summary>
        /// 
        /// </summary>
        public Info SwaggerInfo { get; set; }


        /// <inheritdoc />
        public Startup(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            SwaggerInfo = new Info
            {
                Title = GetType().Assembly.GetName().Name,
                Version = configuration["APIVersion"]
            };
        }

        /// <summary>
        /// AspNetCore Configure DI
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddTaskExecutingServer()
                .AddMvc(options => options.Filters.Add<DomainExceptionFilter>())
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining(GetType());

                    fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                })
                .AddJsonOptions(options =>
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            services
                .Configure<ApiBehaviorOptions>(options =>
                {
                    // Note: We want to have a shared error response whenever there is an internal exception or a validation error.
                    // Note: The ValidationException will be caught by the DomainExceptionFilter
                    options.InvalidModelStateResponseFactory = context =>
                        throw new ValidationException(context.ModelState.Select(s =>
                            new ValidationFailure(s.Key,
                                string.Join(Environment.NewLine, s.Value.Errors?.Select(e => e.ErrorMessage)),
                                s.Value.RawValue)));
                })
                .AddSwagger(SwaggerInfo)
                .AddSwaggerGen(options => options.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "JG.FinTechTest.xml")))
                .AddCors(options =>
                {
                    options.AddPolicy(DefaultCorsPolicy,
                        builder =>
                            builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                // Note: This outputs warning: The CORS protocol does not allow specifying a wildcard (any) origin and credentials at the same time. Configure the policy by listing individual origins if credentials needs to be supported
                                .AllowCredentials());
                });

            var containerBuilder = new ContainerBuilder();

            containerBuilder.Populate(services);

            containerBuilder.RegisterType<LiteDbStartupTask>().As<IStartupTask>();

            // ReSharper disable RedundantNameQualifier
            containerBuilder.RegisterModule<Infrastructure.Logging.Module>();
            containerBuilder.RegisterModule<Infrastructure.Correlation.Module>();
            containerBuilder.RegisterModule<Infrastructure.AspNetCore.Module>();
            containerBuilder.RegisterModule<Domain.Module>();
            // ReSharper restore RedundantNameQualifier

            return new AutofacServiceProvider(containerBuilder.Build());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger(GetType());

            logger.LogInformation("Configure application");

            if (!env.IsDevelopment())
                app.UseHsts();

            app.UseHttpsRedirection();

            // Note: This will branch out the request pipeline. We don't need authorization for Swagger.
            // Note: Swagger is only for development, so it doesn't need Correlation, Authorization etc. This should be a different asp.net core pipeline branch.
            app.MapWhen(x => x.Request.Path.Value.StartsWith("/swagger"), swaggerApp =>
            {
                swaggerApp.UseSwaggerUi(SwaggerInfo);
            });


            // Note: Middleware Order: 1. For services behind reverse proxy. By convention all reverse proxies forward headers from the original request as headers named x-forwarded-*
            app.UseForwardedHeaders();
            // Mote: Middleware Order: 2. Preflight requests do not need to be authenticated. 
            app.UseCors("AllowAllOrigins");
            // Note: Middleware Order: 3. Adds the correlation id headers
            app.UseCorrelationId();
            // Note: Middleware Order: 4. Global error handling comes immediately after Correlation, in order for log entries to have correlation info
            app.UseSafeExceptions();
            // Note: Middleware Order: 5. This should come immediately after Global error handling. This middleware logs unhandled exceptions, but does not handle them.
            app.UseRequestLogging();


            // Note: Add other middleware here. Order is VERY important!

            app.UseMvc();
        }
    }
}

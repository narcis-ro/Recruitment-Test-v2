using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace JG.Infrastructure.AspNetCore.Swagger
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services, Info info)
        {
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc(info.Version ?? "v1", new Info
                {
                    Title = info.Title,
                    Version = info.Version ?? "v1",
                    Description = info.Description ?? "",
                    TermsOfService = info.TermsOfService ?? ""
                });
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerUi(this IApplicationBuilder applicationBuilder, Info info)
        {
            applicationBuilder.UseSwagger()
                .UseSwaggerUI(c => { c.SwaggerEndpoint($"{info.Version ?? "v1"}/swagger.json", info.Title); });
            return applicationBuilder;
        }
    }
}

namespace Roaster.Infrastructure.OpenApi.Extensions
{
    internal static class AspNetCoreIntegration
    {
        internal static IServiceCollection AddCustomOpenApi(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        internal static WebApplication UseOpenApi(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    }
}

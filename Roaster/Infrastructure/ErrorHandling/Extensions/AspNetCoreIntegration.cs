namespace Roaster.Infrastructure.ErrorHandling.Extensions
{
    internal static class AspNetCoreIntegration
    {
        internal static IServiceCollection AddCustomErrorHandling(this IServiceCollection services)
        {
            return services;
        }

        internal static WebApplication UseCustomErrorHandling(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            return app;
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Roaster.Infrastructure.Persistence.Extensions
{
    internal static class AspNetCoreIntegration
    {
        internal static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(db =>
            {
                db.UseSqlServer(configuration.GetConnectionString("RoastDb"), options =>
                {
                    options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly);
                });
            });

            return services;
        }

        internal static async Task MigrateDatabaseAsync(this WebApplication application)
        {
            using (var scope = application.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                try
                {
                    context.Database.EnsureCreated();
                    await context.Database.MigrateAsync();
                }
                catch (Exception exception)
                {
                    logger.LogCritical(exception, "Unable to migrate the database.");
                    throw;
                }
            }
        }
    }
}

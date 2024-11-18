using Microsoft.AspNetCore.DataProtection;
using Roaster.Infrastructure.Persistence;

namespace Roaster.Infrastructure.Protection.Extensions
{
    internal static class AspNetCoreIntegration
    {
        internal static IServiceCollection AddCustomDataProtection(this IServiceCollection services)
        {
            services.AddDataProtection().PersistKeysToDbContext<ApplicationDbContext>();
            return services;
        }
    }
}

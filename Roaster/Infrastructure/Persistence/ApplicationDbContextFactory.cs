using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Roaster.Infrastructure.Persistence
{
    internal class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        ApplicationDbContext IDesignTimeDbContextFactory<ApplicationDbContext>.CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .AddJsonFile("appsettings.Development.json", true)
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("RoastDb");

            builder.UseSqlServer(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}

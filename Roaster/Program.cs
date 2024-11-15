using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Roaster.Infrastructure.Persistence;

// dotnet ef migrations add Roaster_CreateRoast --context ApplicationDbContext --startup-project Roaster.csproj --project Roaster.csproj -o Infrastructure/Persistence/Migrations

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
var zipkinUrl = builder.Configuration["ZIPKIN_URL"];
var otel = builder.Services.AddOpenTelemetry();

if (!builder.Environment.IsDevelopment())
{
    otel.UseAzureMonitor();
}

// Configure OpenTelemetry Resources with the application name
otel.ConfigureResource(resource => resource
    .AddService(serviceName: builder.Environment.ApplicationName));

// Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
otel.WithMetrics(metrics => metrics
    // Metrics provider from OpenTelemetry
    .AddAspNetCoreInstrumentation()
    // Metrics provides by ASP.NET Core in .NET 8
    .AddMeter("Microsoft.AspNetCore.Hosting")
    .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
    .AddMeter("System.Http")
    .AddMeter("Roaster.Api.Roasts")
    .AddPrometheusExporter()
);

// Add Tracing for ASP.NET Core and our custom ActivitySource and export to Jaeger
otel.WithTracing(tracing =>
{
    tracing.AddAspNetCoreInstrumentation();
    tracing.AddHttpClientInstrumentation();

    if (zipkinUrl is not null)
    {
        tracing.AddZipkinExporter(b =>
        {
            b.Endpoint = new Uri(zipkinUrl);
        });
    }

    if (tracingOtlpEndpoint is not null)
    {
        tracing.AddOtlpExporter(otlpOptions =>
         {
             otlpOptions.Endpoint = new Uri(tracingOtlpEndpoint);
         });
    }
});

builder.Logging.AddOpenTelemetry(log =>
{
    log.IncludeScopes = true;
    log.IncludeFormattedMessage = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(db =>
{
    db.UseSqlServer(builder.Configuration.GetConnectionString("RoastDb"), options =>
    {
        options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly);
    });
});

builder.Services.AddDataProtection().PersistKeysToDbContext<ApplicationDbContext>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapPrometheusScrapingEndpoint();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var logger = services.GetRequiredService<ILogger<Program>>();

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
await app.RunAsync();

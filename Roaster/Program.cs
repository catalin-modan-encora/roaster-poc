using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var tracingOtlpEndpoint = builder.Configuration["OTLP_ENDPOINT_URL"];
var zipkinUrl = builder.Configuration["ZIPKIN_URL"];
var otel = builder.Services.AddOpenTelemetry();

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

// The following line enables Application Insights telemetry collection.
builder.Services.AddApplicationInsightsTelemetry();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapPrometheusScrapingEndpoint();
app.Run();

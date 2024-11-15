using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Roaster.Infrastructure.Telemetry.Extensions
{
    internal static class AspNetCoreIntegration
    {
        internal static IServiceCollection AddCustomTelemetry(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment, ILoggingBuilder logging)
        {
            var tracingOtlpEndpoint = configuration["OTLP_ENDPOINT_URL"];
            var zipkinUrl = configuration["ZIPKIN_URL"];
            var otel = services.AddOpenTelemetry();

            if (!environment.IsDevelopment())
            {
                otel.UseAzureMonitor();
            }

            // Configure OpenTelemetry Resources with the application name
            otel.ConfigureResource(resource => resource
                .AddService(serviceName: environment.ApplicationName));

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

            logging.AddOpenTelemetry(log =>
            {
                log.IncludeScopes = true;
                log.IncludeFormattedMessage = true;
            });

            return services;
        }

        internal static WebApplication UseCustomTelemetry(this WebApplication application)
        {
            application.MapPrometheusScrapingEndpoint();

            return application;
        }
    }
}

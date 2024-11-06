using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddOpenTelemetry(log =>
{
    log.IncludeScopes = true;
    log.IncludeFormattedMessage = true;
});
builder.Services
    .AddOpenTelemetry()
    .WithMetrics(x => {
       x.AddRuntimeInstrumentation();
       x
        .AddMeter("Microsoft.AspNetCore.Hosting")
        .AddMeter("Microsoft.AspNetCore.Server.Kestrel")
        .AddMeter("System.Http")
        .AddMeter("Roaster.Api.Roasts")
        .AddPrometheusExporter();
    })
    .WithTracing(x =>
    {
        if(builder.Environment.IsDevelopment())
        {
            x.SetSampler<AlwaysOnSampler>();
        }

        x.AddAspNetCoreInstrumentation();
        x.AddHttpClientInstrumentation();
    });


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

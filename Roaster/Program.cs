using Roaster.Infrastructure.ErrorHandling.Extensions;
using Roaster.Infrastructure.OpenApi.Extensions;
using Roaster.Infrastructure.Persistence.Extensions;
using Roaster.Infrastructure.Protection.Extensions;
using Roaster.Infrastructure.Telemetry.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddCustomOpenApi();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.AddCustomDataProtection();
builder.Services.AddCustomErrorHandling();
builder.Services.AddCustomTelemetry(builder.Configuration, builder.Environment, builder.Logging);

var app = builder.Build();
app
    .UseCustomErrorHandling()
    .UseHttpsRedirection()
    .UseAuthorization();
app.MapControllers();
app
    .UseCustomTelemetry()
    .UseOpenApi();
await app.RunAsync();

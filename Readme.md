# URLs

## Docker
### Application
- http://localhost:880/WeatherForecast
- http://localhost:880/metrics
### Prometheus
- http://localhost:9090
### Zipkin
- http://localhost:9411

## Development
### Application
- https://localhost:7210
- http://localhost:5144

# Migrations
```
cd Roaster
dotnet ef migrations add Roaster_CreateRoast --context ApplicationDbContext --startup-project Roaster.csproj --project Roaster.csproj -o Infrastructure/Persistence/Migrations
```
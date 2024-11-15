# URLs

## Docker
### Application
- http://localhost:8080/WeatherForecast
- http://localhost:8080/metrics
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

# Git TAG
```
git tag -a 1.1.14 -m "release-1.1.14"
git push --follow-tags
```

# Database authentication using MANID
1. Execute the scripts below on your database (not on master):
```
CREATE USER "mi-webapp-roaster-northeu" FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER "mi-webapp-roaster-northeu";
ALTER ROLE db_dataWRITER ADD MEMBER "mi-webapp-roaster-northeu";
ALTER ROLE db_owner ADD MEMBER [mi-webapp-roaster-northeu]
```
2. Assign the managed identity to the web application.
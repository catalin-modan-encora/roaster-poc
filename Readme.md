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
1. Execute the scripts below on your database (not on masterm without <>). This effective registers the managed identity as a user in the database server.
```
CREATE USER "<MANID name>" FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER "<MANID name>";
ALTER ROLE db_dataWRITER ADD MEMBER "<MANID name>";
ALTER ROLE db_owner ADD MEMBER [<MANID name>]
```
2. Assign the managed identity to the web application.
3. Create the connection string:
```
Server=<server address>,1433;Initial Catalog=<database>;Persist Security Info=False;User ID=<MANID Client ID>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication="Active Directory Managed Identity";
```
4. Make sure that private network access is enable for the VNET/Subvnet of the web app, on the SQL server.
5. vertify that the VNET has the `Microsoft.Sql` service endpoint enabled.
6. Set the environment variable in the web app, having the name `ConnectionStrings__RoastDb` and set its value to the connection string from point (3).
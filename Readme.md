# URLs

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
5. Verify that the VNET has the `Microsoft.Sql` service endpoint enabled.
6. Set the environment variable in the web app, having the name `ConnectionStrings__RoastDb` and set its value to the connection string from point (3).

# Azure setup

1. Use the provided templates to deploy into a subscription.
2. Wait for the resources to be deployed.
3. Go to the web app and download the publish profile.
4. Create a new environment in Github.
5. Set the secrets for the new environment in Github:

   1. Set the `AZURE_WEBAPP_PUBLISH_PROFILE` to the contents of the previously downloaded publish profile. This secret allows the pipeline to deploy to the Azure Web App.
   2. Set the `DATABASE_CONNECTION_STRING` to the connection string of the database. This connection string is used by the CD pipeline to apply migrations. Do not quote the string. Make sure to use single quotes `''` around multi words. For example, a connection string should look like:

   ```
    Server=<server address>,1433;Initial Catalog=<database>;Persist Security Info=False;User ID=<MANID Client ID>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication='Active Directory Managed Identity';
   ```

6. Set environment specific variables in Github:
   1.Set the `AZURE_WEBAPP_NAME` to the resource name of the deployed Azure Web Application.
7. Integrate Github with the ACR:
   1. Create a `push only` token in the ACR. Grant the `repositories_push` scope. Use `github` as default.
   2. Set the `AZURE_CLIENT_ID` secret to the name of the name of the newly created token.
   3. Set the `AZURE_CLIENT_SECRET` secret to the `password1` or `password2` of the newly created ACR token. These passwords need to be manually generated inside the portal.
   4. Set the `ACR_URL` variable to the address of the ACR.
   5. Set the `REPOSITORY` variable to the name of the desired repository. Use `roaster-api` as default.
8. Integrate the web app with the ACR:
   1. Generate a new `pull only` ACR token.
   2. Set the `DOCKER_REGISTRY_SERVER_USERNAME` and `DOCKER_REGISTRY_SERVER_PASSWORD` to the ACR token name and password.
9. Setup the Web App:
10. Set the connection string environment variable. Set `ConnectionStrings__RoastDb` to the connection string you crafted in `Database authentication using MANID`.
11. Set up the `APPLICATIONINSIGHTS_CONNECTION_STRING` to the connection string from the Application Insights resource.

# CourtApp Backend API

This is the backend API for the CourtApp project.

## Database Migration to Render PostgreSQL

This project has been migrated to use PostgreSQL, hosted on Render.

### Connection String Configuration

The PostgreSQL connection string is configured in `appsettings.json` under the `ConnectionStrings:DefaultConnection` key. For local development, you can use a local PostgreSQL instance. For deployment to Render, ensure you configure the following environment variables in your Render service settings:

*   `DB_HOST`: Your Render PostgreSQL host (e.g., `dpg-yourprojectname-postgresql.render.com`)
*   `DB_PORT`: Usually `5432`
*   `DB_NAME`: Your database name (e.g., `your_database_name`)
*   `DB_USER`: Your database username (e.g., `your_username`)
*   `DB_PASSWORD`: Your database password

Example `appsettings.json` entry:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=${DB_HOST};Port=${DB_PORT};Database=${DB_NAME};Username=${DB_USER};Password=${DB_PASSWORD};SSL Mode=Require;Trust Server Certificate=true"
}
```

### Applying Migrations to Render PostgreSQL

To apply database migrations to your Render PostgreSQL instance, ensure your `appsettings.json` `DefaultConnection` is correctly configured with your Render database credentials (or set the environment variables locally for `dotnet ef` commands).

Then, navigate to the `CourtApp.Api` directory and run the following commands:

```bash
dotnet ef database update
```

### Deploying to Render

When deploying this API to Render, ensure you configure the environment variables mentioned above in your Render service settings. Render will automatically pick up these environment variables and use them to connect to your PostgreSQL database.

## Running the API Locally

1.  Ensure you have .NET 8 SDK installed.
2.  Navigate to the `CourtApp.Api` directory.
3.  Update `appsettings.json` with your local SQL Server or PostgreSQL connection string.
4.  Run `dotnet run`.

## Running Tests

To run unit and integration tests, navigate to the `CourtApp.Api.Tests` directory and run:

```bash
dotnet test
```

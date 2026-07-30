# RequirementAI

RequirementAI is a full-stack requirements management app with:

- Angular frontend in `Frontend`
- .NET 9 API in `API/RequirementAI.API`
- .NET worker service in `API/RequirementAI.Workers`
- PostgreSQL persistence via Entity Framework Core migrations
- LLM-backed refinement and quality analysis workflows

## Prerequisites

- Node.js and npm compatible with the Angular version in `Frontend/package.json`
- .NET 9 SDK
- PostgreSQL database
- API keys for the LLM providers you want to use

## Frontend

From the repository root:

```bash
cd Frontend
npm install
ng serve
```

The frontend runs on `http://localhost:4200` by default.

## API

Configure secrets with .NET user secrets. Do not commit real secret values to the repository.

The API project path is:

```bash
API/RequirementAI.API/RequirementAI.API.csproj
```

Set the required values:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=yourHostName;Database=yourDatabaseName;Username=yourUserName;Password=yourPassword;SSL Mode=VerifyFull;Channel Binding=Require;" --project API/RequirementAI.API/RequirementAI.API.csproj

dotnet user-secrets set "Jwt:Key" "your-long-random-signing-key" --project API/RequirementAI.API/RequirementAI.API.csproj

dotnet user-secrets set "Authentication:RegistrationSecret" "your-registration-secret" --project API/RequirementAI.API/RequirementAI.API.csproj
```

Optional values:

```bash
dotnet user-secrets set "AutoMapperOptions:LicenseKey" "your-automapper-license-key" --project API/RequirementAI.API/RequirementAI.API.csproj
```

Run the API:

```bash
dotnet run --project API/RequirementAI.API/RequirementAI.API.csproj --launch-profile https
```

The HTTPS launch profile exposes:

- `https://localhost:7027`
- `http://localhost:5079`

The API applies pending database migrations on startup.

## Workers

The worker project runs background refinement and quality analysis jobs.

The worker project path is:

```bash
API/RequirementAI.Workers/RequirementAI.Workers.csproj
```

Configure the same database connection string used by the API:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=yourHostName;Database=yourDatabaseName;Username=yourUserName;Password=yourPassword;SSL Mode=VerifyFull;Channel Binding=Require;" --project API/RequirementAI.Workers/RequirementAI.Workers.csproj
```

Configure the LLM provider keys needed by `API/RequirementAI.Workers/appsettings.json`:

```bash
dotnet user-secrets set "LLM:Providers:openai-main:ApiKey" "your-openai-api-key" --project API/RequirementAI.Workers/RequirementAI.Workers.csproj
dotnet user-secrets set "LLM:Providers:anthropic-main:ApiKey" "your-anthropic-api-key" --project API/RequirementAI.Workers/RequirementAI.Workers.csproj
dotnet user-secrets set "LLM:Providers:moonshot-main:ApiKey" "your-moonshot-api-key" --project API/RequirementAI.Workers/RequirementAI.Workers.csproj
dotnet user-secrets set "LLM:Providers:google-main:ApiKey" "your-google-api-key" --project API/RequirementAI.Workers/RequirementAI.Workers.csproj
```

You only need keys for providers selected in the `LLM:Routing` section.

Optional value:

```bash
dotnet user-secrets set "AutoMapperOptions:LicenseKey" "your-automapper-license-key" --project API/RequirementAI.Workers/RequirementAI.Workers.csproj
```

Run the workers:

```bash
dotnet run --project API/RequirementAI.Workers/RequirementAI.Workers.csproj
```

## User Secrets Reference

User secret keys follow the JSON path structure of the appsettings.json. For example:

```json
{
  "SomeSection": {
    "SomeSubsection": {
      "SomeValue": "value"
    }
  }
}
```

is configured with:

```bash
dotnet user-secrets set "SomeSection:SomeSubsection:SomeValue" "value" --project path/to/project.csproj
```

List configured secrets:

```bash
dotnet user-secrets list --project API/RequirementAI.API/RequirementAI.API.csproj
dotnet user-secrets list --project API/RequirementAI.Workers/RequirementAI.Workers.csproj
```

## Typical Local Startup

Start these in separate terminals:

```bash
dotnet run --project API/RequirementAI.API/RequirementAI.API.csproj --launch-profile https
```

```bash
dotnet run --project API/RequirementAI.Workers/RequirementAI.Workers.csproj
```

```bash
cd Frontend
ng serve
```

## Deployment

Deploy the application as three separate runtime units:

- Frontend: static Angular build output
- API: ASP.NET Core web app
- Workers: .NET worker service

### Configuration

Do not deploy real values in `appsettings.json`. Provide production values through your hosting provider's secret or environment configuration.

Required API configuration:

- `ConnectionStrings:DefaultConnection`
- `Jwt:Key`
- `Jwt:Issuer`
- `Jwt:Audience`
- `Authentication:RegistrationSecret`
- `Frontend:Url`

Required worker configuration:

- `ConnectionStrings:DefaultConnection`
- LLM provider API keys selected by `LLM:Routing`

Optional configuration:

- `AutoMapperOptions:LicenseKey`
- `Authentication:Google:ClientId`

### API

Publish the API:

```bash
dotnet publish API/RequirementAI.API/RequirementAI.API.csproj -c Release -o publish/api
```

Run the published API with `ASPNETCORE_ENVIRONMENT` set to the target environment, for example `Production`.

The API applies pending Entity Framework migrations on startup. Make sure the API process has database migration permissions, or run migrations separately before starting the API.

### Workers

Publish the workers:

```bash
dotnet publish API/RequirementAI.Workers/RequirementAI.Workers.csproj -c Release -o publish/workers
```

Run the worker process with `DOTNET_ENVIRONMENT` set to the target environment.

The workers must use the same database as the API and must have access to the configured LLM provider keys.

### Frontend

Build the frontend:

```bash
cd Frontend
npm install
ng build
```

Deploy the generated Angular build output from `Frontend/dist` to your static hosting provider.

Set the frontend API base URL through the Angular environment configuration used for the target deployment.

### First User Registration

Registration requires `Authentication:RegistrationSecret`.

Set a strong value before deployment. When creating an initial user, send the same value in the registration request as `registrationSecret`.

After the initial users are created, rotate the registration secret or remove access to the public registration flow if registration should be closed.

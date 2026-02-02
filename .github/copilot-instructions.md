# AI Copilot Instructions for dotnet-codespaces

## Project Overview
This is a .NET 10 sample project demonstrating a full-stack weather application:
- **BackEnd** (ASP.NET Core API): RESTful service serving weather forecast data via OpenAPI-documented endpoints
- **FrontEnd** (Blazor Server): Server-side Blazor web app consuming the backend API
- Both projects run independently on ports 8081 (backend) and 8080 (frontend)

## Architecture & Key Components

### Backend (SampleApp/BackEnd/)
- **Entry point**: [BackEnd/Program.cs](BackEnd/Program.cs) configures OpenAPI/Scalar and maps the `/weatherforecast` GET endpoint
- **Pattern**: Minimal APIs (no controllers) - route handlers defined directly in Program.cs
- **Data flow**: `app.MapGet()` returns `WeatherForecast` records with auto-computed Fahrenheit conversion
- **External integration**: Uses Scalar (Microsoft.AspNetCore.OpenApi) for interactive API documentation at `/scalar`
- **Note**: OpenAPI servers array is cleared for Codespaces port-forwarding compatibility

### Frontend (SampleApp/FrontEnd/)
- **Entry point**: [FrontEnd/Program.cs](FrontEnd.csproj) configures Blazor Server and HttpClient
- **HTTP Client**: [WeatherForecastClient.cs](FrontEnd/Data/WeatherForecastClient.cs) fetches data from backend via `WEATHER_URL` environment variable
- **Data models**: Shared model at [FrontEnd/Data/WeatherForecast.cs](FrontEnd/Data/WeatherForecast.cs)
- **Critical requirement**: `WEATHER_URL` must be set in configuration (throws `InvalidOperationException` if missing)

## Build & Run Commands

Available VS Code tasks:
- `dotnet build SampleApp/FrontEnd/FrontEnd.csproj` - build frontend
- `dotnet publish SampleApp/FrontEnd/FrontEnd.csproj` - publish frontend
- `dotnet watch run --project SampleApp/FrontEnd/FrontEnd.csproj` - frontend with hot reload
- `dotnet build SampleApp/BackEnd/BackEnd.csproj` - build backend
- `dotnet publish SampleApp/BackEnd/BackEnd.csproj` - publish backend
- `dotnet watch run --project SampleApp/BackEnd/BackEnd.csproj` - backend with hot reload

Run both via "Run All" in VS Code debug menu (sets up port forwarding 8080/8081).

## Minimal API Design (Backend)

The backend uses ASP.NET Core Minimal APIs exclusively - no controllers. Key patterns:
- **Route definition**: Use `app.MapGet()` directly in Program.cs to define endpoints
- **Inline handlers**: Business logic can be inline lambdas for simple operations (see `/weatherforecast` endpoint)
- **Naming requirement**: All endpoints must use `.WithName()` for OpenAPI documentation - this is critical for Scalar UI integration
- **Data models**: Use C# records for API response DTOs to enforce immutability (e.g., `WeatherForecast` record includes computed `TemperatureF` property)
- **Scaling pattern**: For complex operations, extract handlers to separate methods/classes, but keep configuration fluent in Program.cs

## Error Handling

- **Configuration errors**: Use `?? throw new InvalidOperationException()` pattern (see `WEATHER_URL` in FrontEnd/Program.cs) to fail fast on missing critical config
- **HTTP pipeline**: BackEnd uses `app.UseHttpsRedirection()` - ensure all clients use HTTPS in production
- **Client-side resilience**: FrontEnd's `WeatherForecastClient.GetForecastAsync()` returns empty array on null response (`?? []`) rather than throwing
- **Development-only endpoints**: OpenAPI/Scalar endpoints only mapped in Development environment (`if (app.Environment.IsDevelopment())`)
- **Logging configuration**: Both projects use `Information` log level for app code, `Warning` for ASP.NET Core framework (see appsettings.json files)

## Blazor Server Architecture

The frontend is built with Blazor Server (server-side rendering with WebSocket real-time updates):
- **Routing**: [App.razor](FrontEnd/App.razor) manages routing with `<Router>` component; pages in `/Pages/` directory with `@page` directives
- **Layout**: [MainLayout.razor](FrontEnd/Shared/MainLayout.razor) wraps all pages; route-specific layouts via `@layout` directive
- **Navigation**: [NavMenu.razor](FrontEnd/Shared/NavMenu.razor) provides site navigation with active page highlighting
- **Global imports**: [_Imports.razor](FrontEnd/_Imports.razor) declares namespaces available to all `.razor` components
- **Component lifecycle**: Use `OnInitializedAsync()` to fetch data on component load (see [FetchData.razor](FrontEnd/Pages/FetchData.razor))
- **Dependency injection**: Use `@inject` directive in `.razor` files or constructor injection in code-behind components
- **Null-coalescing in templates**: Check for null state before rendering collections (e.g., `@if (forecasts == null) { <p>Loading...</p> }`)

### Component Patterns
- **Page components**: Decorated with `@page "/route"` directive; must be in `/Pages/` directory
- **Shared components**: Reusable components in `/Shared/` or `/Components/` directories
- **Code blocks**: `@code { }` block at end of `.razor` file for C# logic; private fields rendered via `@fieldName`
- **Event handling**: `@onclick="MethodName"` for button clicks; `@onchange="MethodName"` for form inputs
- **Two-way binding**: Use `@bind="variable"` for form inputs; triggers `OnChange` event on input
- **Conditional rendering**: `@if`, `@else if`, `@else` for conditional markup; `@if` is evaluated server-side before sending to browser

## Project Conventions

- **Framework version**: .NET 10 (net10.0) - use latest APIs and patterns
- **Nullable**: Enabled in both projects (`<Nullable>enable</Nullable>`) - handle null references explicitly
- **Implicit usings**: Enabled - common namespaces auto-imported
- **Record types**: Used for immutable data models (e.g., `WeatherForecast` record in BackEnd)
- **Naming**: PascalCase for classes/records; camelCase for parameters and local variables
- **Namespace organization**: Match directory structure (e.g., `FrontEnd.Data` namespace for files in `/FrontEnd/Data/` directory)

## Cross-Component Communication

- Frontend connects to backend via `HttpClient` injected through dependency injection
- Backend URL configured via `WEATHER_URL` environment variable (must be set before app startup)
- Both use JSON serialization for request/response bodies
- Ensure backend is running before frontend attempts to fetch `/weatherforecast`

## Testing & Debugging

- Use VS Code's integrated debugger with breakpoints
- Backend API can be tested via Scalar UI at `https://localhost:8081/scalar`
- Frontend available at `https://localhost:8080`
- Hot reload on file save speeds iteration (use `dotnet watch run` tasks)
- Check [appsettings.Development.json](SampleApp/BackEnd/appsettings.Development.json) for dev-specific configurations

## Environment & Deployment Considerations

### Port Configuration
- **Backend**: Runs on HTTP port 8080 (launchSettings.json) but listens on 8081 in actual deployment for Codespaces
- **Frontend**: Runs on HTTP port 8081 (launchSettings.json) but listens on 8080 in actual deployment
- **Critical**: `WEATHER_URL` environment variable must point to backend URL (e.g., `https://localhost:8081` or production API URL)

### OpenAPI/Codespaces Workaround
- The `document.Servers = []` transformation in BackEnd/Program.cs clears the OpenAPI servers array to support Codespaces port forwarding
- Referenced issue: https://github.com/dotnet/aspnetcore/issues/57332
- This prevents client generation tools from using hardcoded server URLs that would fail in port-forwarded environments

### Dependency Injection
- **Backend**: Uses built-in `WebApplication.CreateBuilder()` DI container
- **Frontend**: Uses built-in DI for `WeatherForecastClient` - HttpClient is injected with configured base address
- **Pattern**: Constructor injection for dependencies; LoggerFactory automatically available

### Configuration Sources (in order)
1. `appsettings.json` (base config)
2. `appsettings.{Environment}.json` (environment overrides)
3. Environment variables (highest priority for `WEATHER_URL`)
- Development environment sets `ASPNETCORE_ENVIRONMENT=Development` in launchSettings.json
- FrontEnd requires `DetailedErrors: true` in Development (appsettings.Development.json) for better error messages

### HTTPS Requirements
- Both apps use `app.UseHttpsRedirection()` in production pipeline
- Development environment may use HTTP for faster iteration
- Certificate handling: Handled automatically by .NET dev certificates for local development

## Key Files Reference
- [SampleApp.sln](SampleApp/SampleApp.sln) - solution file
- [BackEnd/Program.cs](SampleApp/BackEnd/Program.cs) - API endpoint definitions
- [FrontEnd/Program.cs](SampleApp/FrontEnd/Program.cs) - Blazor configuration
- [FrontEnd/Data/WeatherForecastClient.cs](SampleApp/FrontEnd/Data/WeatherForecastClient.cs) - HTTP integration

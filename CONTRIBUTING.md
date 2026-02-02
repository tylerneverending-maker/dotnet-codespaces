# Contributing to dotnet-codespaces

Welcome! This guide will help you get started with development and contribute to the project.

## Table of Contents
- [Getting Started](#getting-started)
- [Development Workflow](#development-workflow)
- [Running Tests](#running-tests)
- [Code Style & Conventions](#code-style--conventions)
- [Git Workflow](#git-workflow)
- [Pull Request Process](#pull-request-process)
- [Extending the Project](#extending-the-project)

## Getting Started

### Prerequisites
- .NET 10.0 SDK or later
- Docker and Docker Compose (optional, for containerized development)
- SQL Server 2022 (for database, or use Docker)
- VS Code with C# extension

### Local Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/tylerneverending-maker/dotnet-codespaces.git
   cd dotnet-codespaces
   ```

2. **Option A: Local Development (without Docker)**
   - Ensure SQL Server is running locally
   - Update connection string in `OceanSuprise/BackEnd/appsettings.json` if needed
   - Run both projects using VS Code "Run All" command or:
     ```bash
     # Terminal 1: Backend
     dotnet watch run --project OceanSuprise/BackEnd/BackEnd.csproj
     
     # Terminal 2: Frontend
     dotnet watch run --project OceanSuprise/FrontEnd/FrontEnd.csproj
     ```

3. **Option B: Docker Development**
   ```bash
   docker-compose up -d
   ```
   - Backend: http://localhost:8081
   - Frontend: http://localhost:8080
   - Database: localhost:1433 (sa/YourPassword123!)

### Verify Setup
- Frontend loads at `http://localhost:8080`
- Backend API accessible at `http://localhost:8081/weatherforecast`
- Scalar API docs at `http://localhost:8081/scalar`

## Development Workflow

### 1. Create a Feature Branch
```bash
git checkout -b feature/your-feature-name
# or
git checkout -b fix/your-bug-fix
```

Use descriptive names:
- ✓ `feature/add-humidity-sensor`
- ✓ `fix/forecast-date-calculation`
- ✗ `feature/update` (too vague)

### 2. Make Changes
- Keep commits focused and atomic
- Reference issues in commit messages: `git commit -m "feat: add humidity endpoint (fixes #123)"`
- Use conventional commits: `feat:`, `fix:`, `docs:`, `test:`, `chore:`

### 3. Hot Reload Development
The projects are configured for hot reload:
```bash
dotnet watch run --project OceanSuprise/BackEnd/BackEnd.csproj
```
- Changes save automatically when you modify files
- Browser will refresh (or you can refresh manually)

### 4. Test Your Changes Locally
```bash
# Build solution
dotnet build OceanSuprise/OceanSuprise.sln

# Run tests
dotnet test
```

## Running Tests

### Backend Tests
```bash
dotnet test OceanSuprise/BackEnd.Tests/BackEnd.Tests.csproj
```

Tests cover:
- WeatherService data access layer
- API endpoint functionality
- Database migrations

### Frontend Tests
```bash
dotnet test OceanSuprise/FrontEnd.Tests/FrontEnd.Tests.csproj
```

Tests cover:
- WeatherForecastClient HTTP calls
- Component rendering
- Data binding

### Run All Tests
```bash
dotnet test
```

### Test Coverage
- Aim for >70% coverage on new code
- Use `dotnet test --collect:"XPlat Code Coverage"` for detailed reports

## Code Style & Conventions

### Naming
- **Classes/Records**: `PascalCase` (e.g., `WeatherForecast`, `IWeatherService`)
- **Methods**: `PascalCase` (e.g., `GetForecastAsync()`)
- **Properties**: `PascalCase` (e.g., `TemperatureC`)
- **Variables**: `camelCase` (e.g., `forecastDate`, `tempC`)
- **Constants**: `UPPER_CASE` (e.g., `MAX_FORECAST_DAYS`)

### Nullable References
Enable `<Nullable>enable</Nullable>` is enabled in all projects. Always:
- Mark reference types as nullable: `string?`
- Use null-coalescing: `?? throw new InvalidOperationException()`
- Check before use: `if (value != null)`

### Record Types
Use records for immutable DTOs and data models:
```csharp
public record WeatherForecast
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}
```

### Service Pattern
Always use dependency injection for services:
```csharp
public class MyHandler
{
    private readonly IWeatherService _service;
    
    public MyHandler(IWeatherService service)
    {
        _service = service;
    }
}
```

### Error Handling
Fail fast on critical configuration:
```csharp
var url = configuration["WEATHER_URL"] 
    ?? throw new InvalidOperationException("WEATHER_URL is not configured");
```

Return empty collections instead of throwing:
```csharp
public async Task<WeatherForecast[]> GetForecastAsync(DateTime? startDate)
    => await _context.WeatherForecasts.ToArrayAsync() ?? [];
```

## Git Workflow

### 1. Keep Your Branch Updated
```bash
git fetch origin
git rebase origin/main
```

### 2. Before Opening a PR
```bash
# Format code
dotnet format OceanSuprise/OceanSuprise.sln

# Build and test
dotnet build OceanSuprise/OceanSuprise.sln
dotnet test
```

### 3. Push to GitHub
```bash
git push origin feature/your-feature-name
```

## Pull Request Process

### Create a PR with:
1. **Descriptive title**: "Add humidity sensor to weather forecast API"
2. **Summary of changes**: What you changed and why
3. **Related issues**: "Fixes #123"
4. **Type of change**: Bug fix / Feature / Documentation
5. **Testing done**: "Added tests for new endpoint, all tests passing"

### Before merge, ensure:
- ✓ All GitHub Actions checks pass (build, code quality)
- ✓ Code review approved (if required)
- ✓ No merge conflicts
- ✓ Tests added for new functionality
- ✓ Documentation updated (if needed)

## Extending the Project

### Add a New API Endpoint

1. **Create the service method** in `BackEnd/Data/WeatherService.cs`:
   ```csharp
   public async Task<WeatherForecast?> GetForecastByIdAsync(int id)
   {
       return await _context.WeatherForecasts.FirstOrDefaultAsync(f => f.Id == id);
   }
   ```

2. **Update the interface** `IWeatherService`:
   ```csharp
   Task<WeatherForecast?> GetForecastByIdAsync(int id);
   ```

3. **Add the endpoint** in `BackEnd/Program.cs`:
   ```csharp
   app.MapGet("/weatherforecast/{id}", async (int id, IWeatherService service) =>
   {
       var forecast = await service.GetForecastByIdAsync(id);
       return forecast is null ? Results.NotFound() : Results.Ok(forecast);
   })
   .WithName("GetWeatherForecastById")
   .WithOpenApi();
   ```

4. **Add tests** in `BackEnd.Tests/WeatherServiceTests.cs`:
   ```csharp
   [Fact]
   public async Task GetForecastByIdAsync_WithValidId_ReturnsForecast()
   {
       // Arrange
       // Act
       // Assert
   }
   ```

### Add a New Blazor Component

1. **Create component** in `FrontEnd/Components/MyComponent.razor`:
   ```razor
   @page "/mycomponent"
   @using FrontEnd.Data
   @inject WeatherForecastClient Client
   
   <h3>My Component</h3>
   
   @code {
       protected override async Task OnInitializedAsync()
       {
           // Load data
       }
   }
   ```

2. **Add to navigation** in `FrontEnd/Shared/NavMenu.razor`

3. **Add tests** in `FrontEnd.Tests/Components/MyComponentTests.cs`

### Database Changes (Migrations)

1. **Update entity** in `BackEnd/Data/WeatherDbContext.cs`
2. **Create migration**:
   ```bash
   dotnet ef migrations add AddMyColumn --project OceanSuprise/BackEnd
   ```
3. **Migration auto-applies** on startup via `dbContext.Database.Migrate()`

## Common Commands

```bash
# Build
dotnet build OceanSuprise/OceanSuprise.sln

# Run backend with watch
dotnet watch run --project OceanSuprise/BackEnd/BackEnd.csproj

# Run frontend with watch
dotnet watch run --project OceanSuprise/FrontEnd/FrontEnd.csproj

# Run all tests
dotnet test

# Format code
dotnet format OceanSuprise/OceanSuprise.sln

# Create migration
dotnet ef migrations add MigrationName --project OceanSuprise/BackEnd

# Docker: Start all services
docker-compose up -d

# Docker: View logs
docker-compose logs -f

# Docker: Stop services
docker-compose down
```

## Need Help?

- Check [.github/copilot-instructions.md](.github/copilot-instructions.md) for architecture details
- Read [readme.md](../readme.md) for project overview
- Open an issue on GitHub
- Ask in discussions or create a PR with questions

## Code Review Checklist

Before reviewing, ensure PR has:
- ✓ Meaningful title and description
- ✓ Tests for new functionality
- ✓ Follows .NET conventions (naming, nullable, etc.)
- ✓ No warnings in build (`TreatWarningsAsErrors=true`)
- ✓ Updated documentation if needed
- ✓ All CI/CD checks passing

## Questions?

Welcome to ask! Open an issue, start a discussion, or create a draft PR to collaborate.

Happy coding! 🚀

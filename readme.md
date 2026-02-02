# .NET 10 Weather Application

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen)](https://github.com/dotnet/dotnet-codespaces/actions)
[![Test Coverage](https://img.shields.io/badge/tests-17%20passing-blue)](OceanSuprise/)
[![.NET Version](https://img.shields.io/badge/.NET-10.0-purple)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)

A modern full-stack web application demonstrating .NET 10 best practices with a **RESTful weather API** and **interactive Blazor Server frontend**, containerized for production deployment.

## Quick Start

### 🚀 Local Development (1 minute)

```bash
# Terminal 1: Run Backend API
dotnet watch run --project OceanSuprise/BackEnd/BackEnd.csproj

# Terminal 2: Run Frontend UI  
dotnet watch run --project OceanSuprise/FrontEnd/FrontEnd.csproj
```

- **API**: http://localhost:8081 → Interactive docs at `/scalar`
- **Web App**: http://localhost:8080

### 🐳 Docker Compose (2 commands)

```bash
docker-compose up -d
```

- **API**: http://localhost:8081
- **Web App**: http://localhost:8080  
- **Database**: SQL Server 2022 (auto-initialized with test data)

### ☁️ GitHub Codespaces

[![Open in GitHub Codespaces](https://img.shields.io/static/v1?style=for-the-badge&label=GitHub+Codespaces&message=Open&color=lightgrey&logo=github)](https://codespaces.new/github/dotnet-codespaces)

1. Click the badge above or [create a new Codespace](https://codespaces.new/github/dotnet-codespaces)
2. VS Code opens → Click **Run All** in the debug toolbar
3. Access http://localhost:8080 (frontend) and http://localhost:8081/scalar (API)

## Architecture

```
┌─────────────────────────────────────────────────────────┐
│  Frontend (Blazor Server)                               │
│  ├─ Pages: FetchData.razor (interactive UI)             │
│  ├─ Client: HttpClient → Backend API                    │
│  └─ Port: 8080                                          │
└────────────────┬────────────────────────────────────────┘
                 │ REST (HTTPS)
┌────────────────▼────────────────────────────────────────┐
│  Backend (ASP.NET Core Minimal APIs)                    │
│  ├─ GET    /weatherforecast (list forecasts)            │
│  ├─ POST   /weatherforecast (create)                    │
│  ├─ PUT    /weatherforecast/{id} (update)               │
│  ├─ DELETE /weatherforecast/{id} (delete)               │
│  ├─ GET    /health/live (liveness probe)                │
│  ├─ GET    /health/ready (readiness probe)              │
│  ├─ Port: 8081 | OpenAPI Docs: /scalar                 │
│  └─ Service: IWeatherService (data access layer)        │
└────────────────┬────────────────────────────────────────┘
                 │ EF Core (LINQ)
┌────────────────▼────────────────────────────────────────┐
│  Database (SQL Server 2022)                             │
│  └─ Table: WeatherForecasts (seeded with test data)     │
└─────────────────────────────────────────────────────────┘
```

## Key Features

✅ **Modern .NET Stack**
- .NET 10 with nullable reference types enabled
- Minimal APIs pattern (no controllers)
- Dependency injection and configuration management
- Entity Framework Core with SQL Server

✅ **Production Ready**
- Containerized with Docker and Docker Compose
- Health check endpoints (liveness/readiness for orchestrators)
- Comprehensive error handling and logging
- OpenAPI/Scalar interactive documentation

✅ **Full-Stack Development**
- RESTful API with CRUD operations
- Server-side Blazor with real-time WebSocket updates
- Database migrations and seeding
- Environment-specific configuration

✅ **Quality Assurance**
- GitHub Actions CI/CD (build, test, release)
- Comprehensive test suite (17 unit tests)
- Code quality checks (TreatWarningsAsErrors)
- Automated release publishing

✅ **Developer Experience**
- Hot reload for rapid iteration (both backend and frontend)
- Integrated VS Code debugging
- GitHub Codespaces ready
- Comprehensive CONTRIBUTING guide

## Directory Structure

```
├── OceanSuprise/
│   ├── BackEnd/              # ASP.NET Core API
│   │   ├── Program.cs        # API configuration & routes
│   │   ├── Data/
│   │   │   ├── WeatherService.cs      # IWeatherService implementation
│   │   │   └── WeatherDbContext.cs    # Entity Framework context
│   │   ├── Dockerfile        # Multi-stage backend build
│   │   └── BackEnd.csproj
│   │
│   ├── FrontEnd/             # Blazor Server UI
│   │   ├── Program.cs        # Blazor configuration
│   │   ├── Pages/
│   │   │   └── FetchData.razor        # Weather display component
│   │   ├── Data/
│   │   │   └── WeatherForecastClient.cs  # HTTP client
│   │   ├── Dockerfile        # Multi-stage frontend build
│   │   └── FrontEnd.csproj
│   │
│   ├── BackEnd.Tests/        # Backend unit tests (xUnit)
│   └── FrontEnd.Tests/       # Frontend unit tests (xUnit)
│
├── .github/
│   ├── workflows/            # GitHub Actions CI/CD pipelines
│   │   ├── build.yml         # Build & test on push/PR
│   │   ├── code-quality.yml  # Code analysis & warnings
│   │   └── publish-release.yml # Release artifact publishing
│   ├── ISSUE_TEMPLATE/       # GitHub issue templates
│   └── copilot-instructions.md  # AI agent guidance
│
├── docker-compose.yml        # Orchestration (db, backend, frontend)
├── CONTRIBUTING.md           # Developer guide
└── README.md                 # This file
```

## API Endpoints

### Weather Forecasts

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/weatherforecast` | List all forecasts (optional: `?startDate=2024-01-01`) |
| POST | `/weatherforecast` | Create new forecast |
| PUT | `/weatherforecast/{id}` | Update forecast |
| DELETE | `/weatherforecast/{id}` | Delete forecast |

### Health Checks

| Endpoint | Purpose |
|----------|---------|
| `GET /health/live` | Liveness probe (is API running?) |
| `GET /health/ready` | Readiness probe (is database connected?) |

### Documentation

| Endpoint | Purpose |
|----------|---------|
| `GET /scalar` | Interactive API documentation (Scalar UI) |
| `GET /openapi/v1.json` | OpenAPI schema |

## Running Tests

```bash
# Run all tests
dotnet test OceanSuprise/ --configuration Release

# Run with verbose output
dotnet test OceanSuprise/ -v detailed

# Run specific test project
dotnet test OceanSuprise/BackEnd.Tests/BackEnd.Tests.csproj
```

**Current Status**: 17/17 tests passing ✅

## Development Workflows

### Hot Reload Development

Use the `watch` tasks in VS Code for instant reloading on file changes:

```bash
# Backend with auto-reload (port 8081)
dotnet watch run --project OceanSuprise/BackEnd/BackEnd.csproj

# Frontend with auto-reload (port 8080)
dotnet watch run --project OceanSuprise/FrontEnd/FrontEnd.csproj
```

### Database Migrations

Entity Framework migrations are auto-applied on startup. To create a new migration:

```bash
cd OceanSuprise/BackEnd
dotnet ef migrations add MigrationName
```

### Extending the API

See [CONTRIBUTING.md](CONTRIBUTING.md#extending-the-api) for patterns on adding new endpoints, services, and components.

## CI/CD Pipelines

### Build Pipeline (`.github/workflows/build.yml`)
- Runs on: Push to main, Pull requests
- Builds backend and frontend in parallel
- Publishes release artifacts

### Code Quality (`.github/workflows/code-quality.yml`)
- Compiles with `TreatWarningsAsErrors=true`
- Validates code formatting
- Fails PR if warnings detected

### Release Publishing (`.github/workflows/publish-release.yml`)
- Triggered on: Commits to main, tag pushes
- Creates downloadable artifacts
- Generates GitHub Releases with binaries

## Technology Stack

| Component | Technology |
|-----------|-----------|
| **Framework** | .NET 10 |
| **Backend** | ASP.NET Core (Minimal APIs) |
| **Frontend** | Blazor Server |
| **Database** | SQL Server 2022 + EF Core 10 |
| **API Docs** | OpenAPI + Scalar |
| **Testing** | xUnit |
| **Containers** | Docker + Docker Compose |
| **CI/CD** | GitHub Actions |
| **Dev Container** | Ubuntu 24.04 + .NET SDK |

## Configuration

Environment variables (highest priority):

```bash
WEATHER_URL=https://api.example.com:8081       # Frontend → Backend communication
ASPNETCORE_ENVIRONMENT=Development              # Development or Production
ConnectionStrings__DefaultConnection=...        # Database connection
```

Default configuration in `appsettings.json`:
- Backend: Port 8081, SQL Server at `Server=db;...`
- Frontend: Port 8080, Backend at `http://localhost:8081`

See [.github/copilot-instructions.md](.github/copilot-instructions.md) for architecture details.

## Contributing

This project welcomes contributions and suggestions. See [CONTRIBUTING.md](CONTRIBUTING.md) for details on:
- Local setup and development workflow
- Running tests
- Code conventions
- Extending the project
- Pull request process

Most contributions require you to agree to a Contributor License Agreement (CLA) declaring that you have the right to grant us the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).

## Trademarks

This project may contain trademarks or logos for projects, products, or services. Authorized use of Microsoft trademarks or logos is subject to [Microsoft's Trademark & Brand Guidelines](https://www.microsoft.com/en-us/legal/intellectualproperty/trademarks/usage/general).

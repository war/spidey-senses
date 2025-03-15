# 🕷️ Robot Spiders Control System

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4.svg)](https://dotnet.microsoft.com/download)
[![CI/CD](https://img.shields.io/badge/CI%2FCD-GitHub_Actions-blue.svg)](https://github.com/features/actions)
[![API Documentation](https://img.shields.io/badge/API-Documented-brightgreen.svg)](https://swagger.io/)

A robust, production-ready system for controlling robotic spiders that explore micro fractures on building walls. This project follows clean architecture principles and includes extensive test coverage.

## 📚 Project Overview

This application simulates a control system for robotic spiders that navigate grid-based walls. Each spider is controlled through a series of movement commands sent from a central control station. The spiders follow these commands while staying within the boundaries of the wall.

### ✨ Key Features

- ✅ Process navigation commands for robotic spiders
- ✅ Validate spider movements to prevent falling off walls
- ✅ Console application for direct interaction
- ✅ RESTful API for programmatic access
- ✅ Health monitoring endpoints
- ✅ Comprehensive test suite with 95%+ coverage
- ✅ Auto deploying [test coverage report](https://war.github.io/spidey-senses/)
- ✅ API versioning
- ✅ Error catalog with standardized error responses

## 🏗️ Architecture

This application is built using Clean Architecture principles with the following layers:

```
SpiderControl/
├── src/
│   ├── api/                            # API Projects
│   │   ├── SpiderControl.WebApiV2/     # ASP.NET Core Web API
│   │   └── SpiderControl.Api.Shared/   # Shared API components
│   ├── backend/                        # Core Business Logic
│   │   ├── SpiderControl.Core/         # Domain models and interfaces
│   │   └── SpiderControl.Application/  # Application services
│   └── frontend/                       # Client Applications
│       ├── SpiderControl.Console/      # Console client
│       └── angular-js/                 # Angular web client
│
├── tests/                              # Test Projects
│   ├── SpiderControl.Core.Tests/
│   ├── SpiderControl.Application.Tests/
│   ├── SpiderControl.WebApiV2.Tests/
│   └── ...
│
├── infra/                              # Infrastructure as Code
│   ├── docker/                         # Docker configuration
│   │   ├── api/                        # API container definitions
│   │   ├── frontend/                   # Frontend container definitions
│   ├── k8s/                            # Kubernetes manifests
│   │   ├── base/                       # Base configurations
│   │   ├── overlays/                   # Environment-specific configurations
│   │   └── helm/                       # Helm charts for deployment
│   └── terraform/                      # Infrastructure provisioning
│       ├── modules/                    # Reusable infrastructure components
│       ├── environments/               # Environment configurations
│       └── ci/                         # CI/CD infrastructure
│
├── .dockerignore                       # Docker ignore file
├── .gitignore                          # Git ignore file
├── docker-compose.yml                  # Docker Compose for local development
└── SpiderControl.sln                   # Main solution file

```

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK or later
- Editor (Visual Studio 2022, VS Code, Rider, etc)

### Building the Project
```bash
# Clone the repository
git clone https://github.com/war/spidey-senses.git
cd spidey-senses

# Build the solution
dotnet build

# Run tests
dotnet test

# Run the Console application
cd src/frontend/SpiderControl.Console
dotnet run

# Run the API
cd src/api/SpiderControl.WebApiV2
dotnet run
```

## 🔍 API Documentation

The API is fully documented using Swagger/OpenAPI. When running the API locally, navigate to:
```
https://localhost:5001/swagger
```
(change the port in src/api/SpiderControl.WebApiV2/Properties/launchsettings.json)

### API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/v1/spider/process` | POST | Process spider commands |
| `/api/health/check` | GET | Health check endpoint |
| `/api/health/liveness` | GET | Container liveness probe |
| `/api/health/readiness` | GET | Service readiness probe |
| `/api/version` | GET | API version information |
| `/api/metrics` | GET | Runtime metrics |
| `/api/status` | GET | System status |
| `/api/errors` | GET | Error catalog |
| `/api/config` | GET | Public API configuration |

### Command Protocol

Spider commands follow a simple protocol:
- `F` - Move Forward one grid point in the current direction
- `L` - Rotate 90 degrees to the Left (counter-clockwise)
- `R` - Rotate 90 degrees to the Right (clockwise)

Example request:
```json
{
  "wallInput": "7 15",
  "spiderInput": "2 4 Left",
  "commandInput": "FLFLFRFFLF"
}
```

Example response:
```json
{
  "finalPosition": "3 1 Right"
}
```

## 🧪 Testing Approach

The project follows a comprehensive testing strategy:

- **Unit Tests**: Testing individual components in isolation
- **Integration Tests**: Testing component interactions
- **API Tests**: End-to-end testing of API endpoints

Key testing patterns used:
- Dependency Injection for testability
- Mock objects (using Moq)
- Fluent assertions
- Theory-based parameterized tests
- Test fixtures for shared context

## 🛠️ Technical Highlights

### Tech Stack
| Backend | Frontend | Testing & QA | DevOps |
|---------|----------|--------------|--------|
| **.NET 9** - Latest runtime | **Angular** - SPA framework | **xUnit** - Testing framework | **Docker** - Containerization |
| **ASP.NET Core** - Web API | **TypeScript** - Type-safe JS | **Moq** - Mocking framework | **CI/CD Pipeline** - Automation |
| **MediatR** - CQRS pattern | **TailwindCSS** - UI styling | **GitHub Actions** - CI | **Health Monitoring** - Production |
| **FluentValidation** - Validation | **Responsive Design** - All devices | **98% Code Coverage** - Quality | **Metrics Collection** - Performance |

### Patterns & Principles
- Command Pattern for executing spider movements
- Result Pattern for elegant error handling
- Dependency Injection throughout
- SOLID principles
- Clean Architecture

### Technologies
- .NET 9.0 (C# 13)
- ASP.NET Core for the Web API
- Swagger/OpenAPI for API documentation
- FluentValidation for validation rules
- MediatR for CQRS-style request handling
- Serilog for structured logging
- Health checks for monitoring

### Architecture Decisions
- Feature-based folder organization
- Middleware for centralized error handling
- Versioned API endpoints
- Clean separation of concerns with layered architecture
- Immutable DTOs using C# records

## 📝 Command Examples

### Example 1
```
Input:
- Wall: 7 15
- Spider: 2 4 Left
- Commands: FLFLFRFFLF

Output:
- Final position: 3 1 Right
```

### Example 2
```
Input:
- Wall: 5 5
- Spider: 0 0 Up
- Commands: FFRFFFRRLF

Output:
- Final position: 3 1 Down
```

## 🔄 CI/CD Pipeline

The project uses GitHub Actions for CI/CD with the following workflow:
1. Build the solution
2. Run unit and integration tests
3. Generate code coverage report
4. Build and publish Docker image
5. Deploy to staging (coming soon)
6. Run E2E tests (coming soon)
7. Deploy to production (manual approval) (coming soon)

## 🔮 Future Enhancements

- [x] Web API v2 to get/use results from SpiderControl.Application to move the web UI spider around - done
- [x] Basic Angular UI for spider inputs and grid
- [ ] GraphQL with Hot Chocolate (2nd API implementation)
- [ ] gRPC (3rd API implementation)
- [ ] Minimal APIs (4th API implementation)
- [ ] FastEndpoints (5th API implementation)
- [ ] Serilog logging
- [x] Docker
- [x] Docker-compose
- [ ] Kubernetes - in progress
- [ ] Terraform - in progress
- [ ] Enhanced telemetry and monitoring
- [ ] Process spider movements with history (UI)
- [ ] Process spider movements with history (API)

## 👤 Author

war - [GitHub](https://github.com/war) - [LinkedIn]

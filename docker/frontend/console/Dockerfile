FROM mcr.microsoft.com/dotnet/sdk:9.0 AS copy-proj-files

# Copy solution and project files
COPY ["src/frontend/SpiderControl.Console/SpiderControl.Console.csproj", "src/frontend/SpiderControl.Console/"]
COPY ["src/backend/SpiderControl.Application/SpiderControl.Application.csproj", "src/backend/SpiderControl.Application/"]
COPY ["src/backend/SpiderControl.Core/SpiderControl.Core.csproj", "src/backend/SpiderControl.Core/"]

FROM copy-proj-files AS restore

WORKDIR /src
# Restore dependencies
RUN dotnet restore "frontend/SpiderControl.Console/SpiderControl.Console.csproj"

FROM restore AS copy-src-files

# Copy all source files
COPY src/backend ./backend

FROM copy-src-files AS build

# Build the project
WORKDIR "/src/frontend/SpiderControl.Console"
RUN dotnet build "SpiderControl.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpiderControl.Console.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "SpiderControl.Console.dll"]

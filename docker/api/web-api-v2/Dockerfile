# Summary:
# 1. Copy over project files
# 2. Get nuget packages
# 3. Copy over the rest of the source files
# 4. Build and publish WebApiV2
# 5. Build stage checkpoint (build-output)
# 6. Expose http/https ports
# 7. Copy artifacts/built files from the build-output stage
# 8. Run API

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dependencies
WORKDIR /src

COPY ["src/api/SpiderControl.Api.Shared/SpiderControl.Api.Shared.csproj", "api/SpiderControl.Api.Shared/"]
COPY ["src/api/SpiderControl.WebApiV2/SpiderControl.WebApiV2.csproj", "api/SpiderControl.WebApiV2/"]
COPY ["src/backend/SpiderControl.Application/SpiderControl.Application.csproj", "backend/SpiderControl.Application/"]
COPY ["src/backend/SpiderControl.Core/SpiderControl.Core.csproj", "backend/SpiderControl.Core/"]

# Restore dependencies
RUN dotnet restore "api/SpiderControl.WebApiV2/SpiderControl.WebApiV2.csproj"

# Copy all source files
COPY src/api ./api
COPY src/backend ./backend

FROM dependencies AS build

# Build the project
WORKDIR "/src/api/SpiderControl.WebApiV2"
RUN dotnet build "SpiderControl.WebApiV2.csproj" -c Release -o /app/build

# Publish the application
RUN dotnet publish "SpiderControl.WebApiV2.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS publish

EXPOSE 80
EXPOSE 443

# Copy files from build
WORKDIR /app
COPY --from=build /app/publish .

# Run Web API V2
ENTRYPOINT ["dotnet", "SpiderControl.WebApiV2.dll"]

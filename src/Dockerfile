# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY WeatherMCP.slnx .
COPY WeatherMCP/WeatherMCP.csproj WeatherMCP/

# Restore dependencies
RUN dotnet restore WeatherMCP/WeatherMCP.csproj

# Copy source code
COPY . .

# Build the application
WORKDIR /src/WeatherMCP
RUN dotnet build -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy published application
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5050
ENV ASPNETCORE_ENVIRONMENT=Production

# Expose port 5050
EXPOSE 5050

# Entry point
ENTRYPOINT ["dotnet", "WeatherMCP.dll"]

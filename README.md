# Weather MCP Server

A Model Context Protocol (MCP) server providing comprehensive weather data through NOAA and Open Meteo APIs. This server exposes weather tools that can be consumed by MCP-compatible clients, enabling AI assistants to retrieve real-time weather conditions, forecasts, radar information, and marine data.

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Quick Start with Docker](#quick-start-with-docker)
- [Docker Configuration](#docker-configuration)
- [MCP Client Configuration](#mcp-client-configuration)
- [API Endpoints](#api-endpoints)
- [MCP Tools Reference](#mcp-tools-reference)
  - [NOAA Weather Tools](#noaa-weather-tools)
  - [Open Meteo Marine Tools](#open-meteo-marine-tools)
- [Development](#development)
- [Troubleshooting](#troubleshooting)

## Features

- **NOAA Weather Integration**: Current conditions, 7-day forecasts, and radar information for US locations
- **Marine Weather Data**: Wave heights, swell information, ocean currents, and sea temperatures via Open Meteo
- **Geocoding Support**: Automatic location resolution from city names or US zip codes
- **MCP Protocol**: Full SSE transport support for real-time communication with MCP clients
- **Docker Ready**: Containerized deployment with health checks and security best practices

## Prerequisites

- [Docker](https://docs.docker.com/get-docker/) (version 20.10 or later)
- [Docker Compose](https://docs.docker.com/compose/install/) (optional, for easier management)

## Quick Start with Docker
```bash
docker pull saltystag/weather-mcp-server
```

### Building the Docker Image

Navigate to the project directory and build the image:

```bash
cd src/WeatherMCP
docker build -t weather-mcp-server .
```

### Running the Container

Start the container with port 5050 exposed:

```bash
docker run -d \
  --name weather-mcp \
  -p 5050:5050 \
  --restart unless-stopped \
  weather-mcp-server
```

### Verify the Server is Running

Check the health endpoint:

```bash
curl http://localhost:5050/health
```

Expected response:
```json
{
  "status": "Healthy",
  "timestamp": "2024-12-24T12:00:00Z"
}
```

Get server information:

```bash
curl http://localhost:5050/info
```

### Stopping the Container

```bash
docker stop weather-mcp
docker rm weather-mcp
```

## Docker Configuration

### Using Docker Compose

Create a `docker-compose.yml` file for easier management:

```yaml
version: '3.8'

services:
  weather-mcp:
    build:
      context: .
      dockerfile: Dockerfile
    container_name: weather-mcp-server
    ports:
      - "5050:5050"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
    restart: unless-stopped
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:5050/health"]
      interval: 30s
      timeout: 10s
      retries: 3
      start_period: 10s
```

Run with Docker Compose:

```bash
docker-compose up -d
```

### Environment Variables

| Variable | Default | Description |
|----------|---------|-------------|
| `ASPNETCORE_URLS` | `http://+:5050` | The URL the server listens on |
| `ASPNETCORE_ENVIRONMENT` | `Production` | Runtime environment |

### Custom Port Configuration

To run on a different host port (e.g., 8080):

```bash
docker run -d -p 8080:5050 --name weather-mcp weather-mcp-server
```

## MCP Client Configuration

### Claude Desktop / Claude AI

Add the following to your MCP configuration file (`.mcp.json`):

```json
{
  "servers": {
    "WeatherMCP": {
      "type": "sse",
      "url": "http://localhost:5050",
      "headers": {}
    }
  }
}
```

### Generic MCP Client

Connect to the SSE endpoint at:
- **SSE Endpoint**: `http://localhost:5050/sse`
- **MCP Endpoint**: `http://localhost:5050/mcp`

## API Endpoints

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/health` | GET | Health check endpoint |
| `/info` | GET | Server information and available endpoints |
| `/mcp` | POST | MCP protocol endpoint |
| `/sse` | GET | Server-Sent Events endpoint for MCP |
| `/swagger` | GET | Swagger UI (Development only) |

## MCP Tools Reference

The Weather MCP Server exposes 10 tools for weather data retrieval.

### NOAA Weather Tools

These tools provide weather data for US locations using the National Oceanic and Atmospheric Administration (NOAA) API.

#### `GetCurrentConditions`

Get current weather conditions for a location.

**Parameters:**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| `location` | string | Yes | City name (e.g., "New York") or US zip code (e.g., "32541") |

**Returns:** Current temperature, humidity, wind speed/direction, barometric pressure, visibility, and conditions description.

**Example Response:**
```json
{
  "location": "Destin, Florida, United States",
  "observationTime": "2024-12-24T10:00:00Z",
  "description": "Partly Cloudy",
  "temperatureF": 65.2,
  "temperatureC": 18.4,
  "humidity": 72.0,
  "windSpeed": "8.5 mph",
  "windDirection": "NNE",
  "barometricPressureInHg": 30.12,
  "visibilityMiles": 10.0
}
```

---

#### `GetForecast`

Get 7-day weather forecast for a location.

**Parameters:**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| `location` | string | Yes | City name or US zip code |

**Returns:** Array of forecast periods with temperature, precipitation chance, wind, and detailed forecast text.

**Example Response:**
```json
{
  "location": "Seattle, Washington, United States",
  "generatedAt": "2024-12-24T12:00:00Z",
  "periods": [
    {
      "name": "Today",
      "temperature": 48,
      "temperatureUnit": "F",
      "precipitationChance": 30,
      "windSpeed": "10 mph",
      "windDirection": "SW",
      "shortForecast": "Chance Rain",
      "detailedForecast": "A chance of rain after noon..."
    }
  ]
}
```

---

#### `GetRadarInfo`

Get radar station information and radar image URLs.

**Parameters:**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| `location` | string | Yes | City name or US zip code |

**Returns:** Nearest radar station details, status, and URLs for radar images.

**Example Response:**
```json
{
  "location": "Miami, Florida, United States",
  "nearestRadarStation": "KAMX",
  "radarStationName": "Miami",
  "radarStatus": "Active",
  "radarImageUrl": "https://radar.weather.gov/ridge/standard/KAMX_0.gif",
  "radarLoopUrl": "https://radar.weather.gov/ridge/standard/KAMX_loop.gif"
}
```

---

#### `GetConditionsByCoordinates`

Get current weather using specific latitude/longitude coordinates.

**Parameters:**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| `latitude` | double | Yes | Latitude coordinate (e.g., 30.3935) |
| `longitude` | double | Yes | Longitude coordinate (e.g., -86.4958) |

**Returns:** Same as `GetCurrentConditions`

---

#### `GetForecastByCoordinates`

Get 7-day forecast using specific coordinates.

**Parameters:**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| `latitude` | double | Yes | Latitude coordinate |
| `longitude` | double | Yes | Longitude coordinate |

**Returns:** Same as `GetForecast`

---

### Open Meteo Marine Tools

These tools provide marine weather data using the Open Meteo Marine API. Ideal for coastal locations, boating, surfing, and fishing.

#### `GetCurrentMarineConditions`

Get comprehensive current marine conditions including waves, swells, currents, and sea temperature.

**Parameters:**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| `location` | string | Yes | Coastal city name (e.g., "Miami Beach", "Santa Monica") |

**Returns:** Complete marine data including:
- Wave height, direction, and period
- Wind wave information
- Primary, secondary, and tertiary swell data
- Sea surface temperature
- Ocean current velocity and direction

**Example Response:**
```json
{
  "location": "Santa Monica, California, United States",
  "latitude": 34.0195,
  "longitude": -118.4912,
  "observationTime": "2024-12-24T12:00:00Z",
  "waveHeightMeters": 1.2,
  "waveHeightFeet": 3.9,
  "waveDirectionCardinal": "WSW",
  "wavePeriodSeconds": 12.5,
  "swellWaveHeightMeters": 1.0,
  "seaSurfaceTemperatureC": 16.2,
  "seaSurfaceTemperatureF": 61.2,
  "oceanCurrentVelocityKnots": 0.5,
  "oceanCurrentDirectionCardinal": "S"
}
```

---

#### `GetMarineForecast`

Get hourly and daily marine forecast for up to 7 days.

**Parameters:**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| `location` | string | Yes | Coastal city name |
| `forecastDays` | int | No | Number of forecast days (1-7, default: 7) |

**Returns:** Hourly and daily forecast arrays with wave and swell predictions.

---

#### `GetMarineConditionsByCoordinates`

Get current marine conditions for specific ocean coordinates.

**Parameters:**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| `latitude` | double | Yes | Latitude coordinate |
| `longitude` | double | Yes | Longitude coordinate |

**Returns:** Same as `GetCurrentMarineConditions`

---

#### `GetMarineForecastByCoordinates`

Get marine forecast for specific coordinates.

**Parameters:**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| `latitude` | double | Yes | Latitude coordinate |
| `longitude` | double | Yes | Longitude coordinate |
| `forecastDays` | int | No | Number of forecast days (1-7, default: 7) |

**Returns:** Same as `GetMarineForecast`

---

#### `GetWaveConditionsSummary`

Get a simplified wave conditions summary optimized for surfing, boating, or fishing activities.

**Parameters:**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| `location` | string | Yes | Coastal city name |

**Returns:** Easy-to-read summary with wave heights, directions, periods, sea temperature, and current information.

**Example Response:**
```json
{
  "location": "Huntington Beach, California, United States",
  "observationTime": "2024-12-24T12:00:00Z",
  "waves": {
    "height": "1.5m (4.9ft)",
    "direction": "SW (225°)",
    "period": "14.0 seconds"
  },
  "swell": {
    "height": "1.2m (3.9ft)",
    "direction": "SSW (200°)",
    "period": "16.0 seconds"
  },
  "windWaves": {
    "height": "0.3m (1.0ft)",
    "direction": "W"
  },
  "seaTemperature": "17.5°C (63.5°F)",
  "oceanCurrent": {
    "speed": "0.3 knots",
    "direction": "SE"
  }
}
```

## Development

### Running Locally (Without Docker)

```bash
cd src/WeatherMCP/WeatherMCP
dotnet run
```

The server will start at `http://localhost:5050`.

### Accessing Swagger UI

In development mode, Swagger UI is available at:
```
http://localhost:5050/swagger
```

### Building for Different Architectures

Build for ARM64 (e.g., Apple Silicon, Raspberry Pi):

```bash
docker build --platform linux/arm64 -t weather-mcp-server:arm64 .
```

Build for AMD64:

```bash
docker build --platform linux/amd64 -t weather-mcp-server:amd64 .
```

## Troubleshooting

### Container won't start

Check container logs:
```bash
docker logs weather-mcp
```

### Health check failing

Ensure the container has network access and port 5050 is not blocked:
```bash
docker exec weather-mcp curl http://localhost:5050/health
```

### NOAA API errors

NOAA API only works for US locations. For international weather, the marine tools (Open Meteo) provide global coverage.

### Marine data unavailable

Marine data is only available for ocean/sea locations. Inland locations will return an error with a suggestion to try coastal coordinates.

### Connection refused

Verify the container is running and the port mapping is correct:
```bash
docker ps
docker port weather-mcp
```

## License

This project uses the following external APIs:
- [NOAA Weather API](https://www.weather.gov/documentation/services-web-api) - US Government Public Domain
- [Open Meteo](https://open-meteo.com/) - Free for non-commercial use

## Contributing

Contributions are welcome! Please ensure all changes maintain compatibility with the MCP protocol specification.

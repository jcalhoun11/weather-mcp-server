using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using WeatherMCP.Services;

namespace WeatherMCP.Tools;

/// <summary>
/// MCP Tools for Open Meteo Marine Weather API interactions
/// </summary>
[McpServerToolType]
public static class OpenMeteoMarineTools
{
    /// <summary>
    /// Gets current marine conditions for a location
    /// </summary>
    [McpServerTool]
    [Description("Get current marine weather conditions including wave height, wave direction, wave period, swell information, sea surface temperature, ocean currents, and tides. Returns all available marine variables from Open Meteo.")]
    public static async Task<string> GetCurrentMarineConditions(
        IOpenMeteoMarineService marineService,
        IGeocodingService geocodingService,
        [Description("The coastal location to get marine conditions for. Can be a coastal city name (e.g., 'New York', 'Miami Beach', 'Santa Monica') or coordinates near water")] string location,
        CancellationToken cancellationToken)
    {
        var geoLocation = await geocodingService.GetCoordinatesAsync(location, cancellationToken);
        if (geoLocation is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not find location: {location}" });
        }

        var conditions = await marineService.GetCurrentMarineConditionsAsync(
            geoLocation.Latitude,
            geoLocation.Longitude,
            cancellationToken);

        if (conditions is null)
        {
            return JsonSerializer.Serialize(new
            {
                error = $"Could not retrieve marine conditions for {location}. Note: Marine data is only available for ocean/sea locations.",
                suggestion = "Try a coastal location or use coordinates directly in the water."
            });
        }

        conditions.Location = geoLocation.DisplayName;
        return JsonSerializer.Serialize(conditions, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Gets marine forecast for a location
    /// </summary>
    [McpServerTool]
    [Description("Get marine weather forecast including hourly and daily wave, swell, and ocean current predictions. Returns up to 7 days of marine forecast data from Open Meteo.")]
    public static async Task<string> GetMarineForecast(
        IOpenMeteoMarineService marineService,
        IGeocodingService geocodingService,
        [Description("The coastal location to get marine forecast for. Can be a coastal city name (e.g., 'San Diego', 'Key West', 'Myrtle Beach') or coordinates near water")] string location,
        [Description("Number of forecast days (1-7, default is 7)")] int forecastDays = 7,
        CancellationToken cancellationToken = default)
    {
        forecastDays = Math.Clamp(forecastDays, 1, 7);

        var geoLocation = await geocodingService.GetCoordinatesAsync(location, cancellationToken);
        if (geoLocation is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not find location: {location}" });
        }

        var forecast = await marineService.GetMarineForecastAsync(
            geoLocation.Latitude,
            geoLocation.Longitude,
            forecastDays,
            cancellationToken);

        if (forecast is null)
        {
            return JsonSerializer.Serialize(new
            {
                error = $"Could not retrieve marine forecast for {location}. Note: Marine data is only available for ocean/sea locations.",
                suggestion = "Try a coastal location or use coordinates directly in the water."
            });
        }

        forecast.Location = geoLocation.DisplayName;
        return JsonSerializer.Serialize(forecast, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Gets current marine conditions by coordinates
    /// </summary>
    [McpServerTool]
    [Description("Get current marine weather conditions using specific latitude and longitude coordinates. Best for offshore or specific ocean locations. Returns all marine variables including waves, swells, currents, and sea temperature.")]
    public static async Task<string> GetMarineConditionsByCoordinates(
        IOpenMeteoMarineService marineService,
        [Description("Latitude coordinate (e.g., 30.3935 for New York offshore)")] double latitude,
        [Description("Longitude coordinate (e.g., -86.4958 for New York offshore)")] double longitude,
        CancellationToken cancellationToken)
    {
        var conditions = await marineService.GetCurrentMarineConditionsAsync(
            latitude,
            longitude,
            cancellationToken);

        if (conditions is null)
        {
            return JsonSerializer.Serialize(new
            {
                error = $"Could not retrieve marine conditions for coordinates: {latitude}, {longitude}",
                suggestion = "Marine data may not be available at this location. Try coordinates over the ocean."
            });
        }

        return JsonSerializer.Serialize(conditions, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Gets marine forecast by coordinates
    /// </summary>
    [McpServerTool]
    [Description("Get marine weather forecast using specific latitude and longitude coordinates. Includes hourly and daily wave predictions for up to 7 days.")]
    public static async Task<string> GetMarineForecastByCoordinates(
        IOpenMeteoMarineService marineService,
        [Description("Latitude coordinate (e.g., 30.3935)")] double latitude,
        [Description("Longitude coordinate (e.g., -86.4958)")] double longitude,
        [Description("Number of forecast days (1-7, default is 7)")] int forecastDays = 7,
        CancellationToken cancellationToken = default)
    {
        forecastDays = Math.Clamp(forecastDays, 1, 7);

        var forecast = await marineService.GetMarineForecastAsync(
            latitude,
            longitude,
            forecastDays,
            cancellationToken);

        if (forecast is null)
        {
            return JsonSerializer.Serialize(new
            {
                error = $"Could not retrieve marine forecast for coordinates: {latitude}, {longitude}",
                suggestion = "Marine data may not be available at this location. Try coordinates over the ocean."
            });
        }

        return JsonSerializer.Serialize(forecast, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Gets wave conditions summary for surfing/boating
    /// </summary>
    [McpServerTool]
    [Description("Get a summary of wave conditions suitable for surfing, boating, or fishing activities. Returns wave heights, periods, and directions in an easy-to-understand format.")]
    public static async Task<string> GetWaveConditionsSummary(
        IOpenMeteoMarineService marineService,
        IGeocodingService geocodingService,
        [Description("The coastal location to get wave conditions for (e.g., 'Huntington Beach', 'Outer Banks', 'Panama City Beach')")] string location,
        CancellationToken cancellationToken)
    {
        var geoLocation = await geocodingService.GetCoordinatesAsync(location, cancellationToken);
        if (geoLocation is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not find location: {location}" });
        }

        var conditions = await marineService.GetCurrentMarineConditionsAsync(
            geoLocation.Latitude,
            geoLocation.Longitude,
            cancellationToken);

        if (conditions is null)
        {
            return JsonSerializer.Serialize(new
            {
                error = $"Could not retrieve wave conditions for {location}",
                suggestion = "Try a coastal location closer to the ocean."
            });
        }

        var summary = new
        {
            Location = geoLocation.DisplayName,
            ObservationTime = conditions.ObservationTime,
            Waves = new
            {
                Height = $"{conditions.WaveHeightMeters:F1}m ({conditions.WaveHeightFeet:F1}ft)",
                Direction = $"{conditions.WaveDirectionCardinal} ({conditions.WaveDirectionDegrees}°)",
                Period = $"{conditions.WavePeriodSeconds:F1} seconds"
            },
            Swell = new
            {
                Height = $"{conditions.SwellWaveHeightMeters:F1}m ({conditions.SwellWaveHeightFeet:F1}ft)",
                Direction = $"{conditions.SwellWaveDirectionCardinal} ({conditions.SwellWaveDirectionDegrees}°)",
                Period = $"{conditions.SwellWavePeriodSeconds:F1} seconds"
            },
            WindWaves = new
            {
                Height = $"{conditions.WindWaveHeightMeters:F1}m ({conditions.WindWaveHeightFeet:F1}ft)",
                Direction = $"{conditions.WindWaveDirectionCardinal}"
            },
            SeaTemperature = $"{conditions.SeaSurfaceTemperatureC:F1}°C ({conditions.SeaSurfaceTemperatureF:F1}°F)",
            OceanCurrent = new
            {
                Speed = $"{conditions.OceanCurrentVelocityKnots:F1} knots",
                Direction = $"{conditions.OceanCurrentDirectionCardinal}"
            }
        };

        return JsonSerializer.Serialize(summary, new JsonSerializerOptions { WriteIndented = true });
    }
}


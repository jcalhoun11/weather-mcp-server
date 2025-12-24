using System.ComponentModel;
using System.Text.Json;
using ModelContextProtocol.Server;
using WeatherMCP.Services;

namespace WeatherMCP.Tools;

/// <summary>
/// MCP Tools for NOAA Weather API interactions
/// </summary>
[McpServerToolType]
public static class NoaaWeatherTools
{
    /// <summary>
    /// Gets current weather conditions for a city or zip code
    /// </summary>
    [McpServerTool]
    [Description("Get current weather conditions for a location. Returns temperature, humidity, wind, pressure, and other current conditions from NOAA.")]
    public static async Task<string> GetCurrentConditions(
        INoaaWeatherService weatherService,
        IGeocodingService geocodingService,
        [Description("The location to get weather for. Can be a city name (e.g., 'Destin' or 'New York') or a US zip code (e.g., '32541')")] string location,
        CancellationToken cancellationToken)
    {
        // Geocode the location
        var geoLocation = await geocodingService.GetCoordinatesAsync(location, cancellationToken);
        if (geoLocation is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not find location: {location}" });
        }

        // Get current conditions
        var conditions = await weatherService.GetCurrentConditionsAsync(
            geoLocation.Latitude,
            geoLocation.Longitude,
            cancellationToken);

        if (conditions is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not retrieve weather conditions for {location}" });
        }

        // Update location with geocoded name
        conditions.Location = geoLocation.DisplayName;

        return JsonSerializer.Serialize(conditions, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Gets weather forecast for a city or zip code
    /// </summary>
    [McpServerTool]
    [Description("Get the 7-day weather forecast for a location. Returns detailed forecast periods including temperature, precipitation chance, wind, and conditions from NOAA.")]
    public static async Task<string> GetForecast(
        INoaaWeatherService weatherService,
        IGeocodingService geocodingService,
        [Description("The location to get the forecast for. Can be a city name (e.g., 'New York' or 'Seattle') or a US zip code (e.g., '98101')")] string location,
        CancellationToken cancellationToken)
    {
        // Geocode the location
        var geoLocation = await geocodingService.GetCoordinatesAsync(location, cancellationToken);
        if (geoLocation is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not find location: {location}" });
        }

        // Get forecast
        var forecast = await weatherService.GetForecastAsync(
            geoLocation.Latitude,
            geoLocation.Longitude,
            cancellationToken);

        if (forecast is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not retrieve forecast for {location}" });
        }

        // Update location with geocoded name
        forecast.Location = geoLocation.DisplayName;

        return JsonSerializer.Serialize(forecast, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Gets radar information for a city or zip code
    /// </summary>
    [McpServerTool]
    [Description("Get radar information for a location including nearest radar station details and radar image URLs from NOAA.")]
    public static async Task<string> GetRadarInfo(
        INoaaWeatherService weatherService,
        IGeocodingService geocodingService,
        [Description("The location to get radar info for. Can be a city name (e.g., 'Miami, FL' or 'Chicago') or a US zip code (e.g., '33101')")] string location,
        CancellationToken cancellationToken)
    {
        // Geocode the location
        var geoLocation = await geocodingService.GetCoordinatesAsync(location, cancellationToken);
        if (geoLocation is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not find location: {location}" });
        }

        // Get radar info
        var radarInfo = await weatherService.GetRadarInfoAsync(
            geoLocation.Latitude,
            geoLocation.Longitude,
            cancellationToken);

        if (radarInfo is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not retrieve radar info for {location}" });
        }

        // Update location with geocoded name
        radarInfo.Location = geoLocation.DisplayName;

        return JsonSerializer.Serialize(radarInfo, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Gets weather for specific coordinates
    /// </summary>
    [McpServerTool]
    [Description("Get current weather conditions using specific latitude and longitude coordinates from NOAA.")]
    public static async Task<string> GetConditionsByCoordinates(
        INoaaWeatherService weatherService,
        [Description("Latitude coordinate (e.g., 30.3935)")] double latitude,
        [Description("Longitude coordinate (e.g., -86.4958)")] double longitude,
        CancellationToken cancellationToken)
    {
        var conditions = await weatherService.GetCurrentConditionsAsync(latitude, longitude, cancellationToken);

        if (conditions is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not retrieve weather conditions for coordinates: {latitude}, {longitude}" });
        }

        return JsonSerializer.Serialize(conditions, new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Gets forecast for specific coordinates
    /// </summary>
    [McpServerTool]
    [Description("Get the 7-day weather forecast using specific latitude and longitude coordinates from NOAA.")]
    public static async Task<string> GetForecastByCoordinates(
        INoaaWeatherService weatherService,
        [Description("Latitude coordinate (e.g., 30.3935)")] double latitude,
        [Description("Longitude coordinate (e.g., -86.4958)")] double longitude,
        CancellationToken cancellationToken)
    {
        var forecast = await weatherService.GetForecastAsync(latitude, longitude, cancellationToken);

        if (forecast is null)
        {
            return JsonSerializer.Serialize(new { error = $"Could not retrieve forecast for coordinates: {latitude}, {longitude}" });
        }

        return JsonSerializer.Serialize(forecast, new JsonSerializerOptions { WriteIndented = true });
    }
}

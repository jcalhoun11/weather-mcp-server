using WeatherMCP.Models;

namespace WeatherMCP.Services;

/// <summary>
/// Interface for NOAA weather service operations
/// </summary>
public interface INoaaWeatherService
{
    Task<CurrentConditionsResult?> GetCurrentConditionsAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
    Task<ForecastResult?> GetForecastAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
    Task<RadarInfoResult?> GetRadarInfoAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for interacting with NOAA Weather API
/// </summary>
public sealed class NoaaWeatherService : INoaaWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<NoaaWeatherService> _logger;

    public NoaaWeatherService(HttpClient httpClient, ILogger<NoaaWeatherService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Gets current weather conditions for the specified coordinates
    /// </summary>
    public async Task<CurrentConditionsResult?> GetCurrentConditionsAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Step 1: Get point metadata (grid info and observation stations)
            var pointsResponse = await GetPointsDataAsync(latitude, longitude, cancellationToken);
            if (pointsResponse?.Properties is null)
            {
                _logger.LogWarning("No points data found for coordinates: {Lat}, {Lon}", latitude, longitude);
                return null;
            }

            var location = $"{pointsResponse.Properties.RelativeLocation?.Properties?.City}, {pointsResponse.Properties.RelativeLocation?.Properties?.State}";

            // Step 2: Get observation stations
            var stationsUrl = pointsResponse.Properties.ObservationStationsUrl;
            if (string.IsNullOrEmpty(stationsUrl))
            {
                _logger.LogWarning("No observation stations URL found");
                return null;
            }

            var stationsResponse = await _httpClient.GetFromJsonAsync<NoaaStationsResponse>(
                stationsUrl,
                cancellationToken);

            if (stationsResponse?.Features is null || stationsResponse.Features.Count == 0)
            {
                _logger.LogWarning("No observation stations found");
                return null;
            }

            // Step 3: Get latest observation from first station
            var stationId = stationsResponse.Features[0].Properties?.StationIdentifier;
            if (string.IsNullOrEmpty(stationId))
            {
                _logger.LogWarning("No station identifier found");
                return null;
            }

            var observationUrl = $"stations/{stationId}/observations/latest";
            var observation = await _httpClient.GetFromJsonAsync<NoaaObservationResponse>(
                observationUrl,
                cancellationToken);

            if (observation?.Properties is null)
            {
                _logger.LogWarning("No observation data found");
                return null;
            }

            var props = observation.Properties;

            return new CurrentConditionsResult
            {
                Location = location,
                ObservationTime = props.Timestamp,
                Description = props.TextDescription,
                TemperatureC = props.Temperature?.Value,
                TemperatureF = ConvertCelsiusToFahrenheit(props.Temperature?.Value),
                FeelsLikeC = props.WindChill?.Value ?? props.HeatIndex?.Value ?? props.Temperature?.Value,
                FeelsLikeF = ConvertCelsiusToFahrenheit(props.WindChill?.Value ?? props.HeatIndex?.Value ?? props.Temperature?.Value),
                Humidity = props.RelativeHumidity?.Value,
                DewpointC = props.Dewpoint?.Value,
                DewpointF = ConvertCelsiusToFahrenheit(props.Dewpoint?.Value),
                WindSpeed = ConvertMetersPerSecondToMph(props.WindSpeed?.Value)?.ToString("F1") + " mph",
                WindDirection = ConvertDegreesToCardinal(props.WindDirection?.Value),
                WindGustMph = ConvertMetersPerSecondToMph(props.WindGust?.Value),
                BarometricPressureInHg = ConvertPascalsToInHg(props.BarometricPressure?.Value),
                VisibilityMiles = ConvertMetersToMiles(props.Visibility?.Value),
                IconUrl = props.Icon
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching current conditions");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching current conditions");
            return null;
        }
    }

    /// <summary>
    /// Gets 7-day forecast for the specified coordinates
    /// </summary>
    public async Task<ForecastResult?> GetForecastAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Step 1: Get point metadata
            var pointsResponse = await GetPointsDataAsync(latitude, longitude, cancellationToken);
            if (pointsResponse?.Properties is null)
            {
                _logger.LogWarning("No points data found for coordinates: {Lat}, {Lon}", latitude, longitude);
                return null;
            }

            var location = $"{pointsResponse.Properties.RelativeLocation?.Properties?.City}, {pointsResponse.Properties.RelativeLocation?.Properties?.State}";
            var forecastUrl = pointsResponse.Properties.ForecastUrl;

            if (string.IsNullOrEmpty(forecastUrl))
            {
                _logger.LogWarning("No forecast URL found");
                return null;
            }

            // Step 2: Get forecast
            var forecastResponse = await _httpClient.GetFromJsonAsync<NoaaForecastResponse>(
                forecastUrl,
                cancellationToken);

            if (forecastResponse?.Properties is null)
            {
                _logger.LogWarning("No forecast data found");
                return null;
            }

            var periods = forecastResponse.Properties.Periods?.Select(p => new ForecastPeriodResult
            {
                Name = p.Name,
                StartTime = p.StartTime,
                EndTime = p.EndTime,
                IsDaytime = p.IsDaytime,
                Temperature = p.Temperature,
                TemperatureUnit = p.TemperatureUnit,
                PrecipitationChance = (int?)p.ProbabilityOfPrecipitation?.Value,
                Humidity = (int?)p.RelativeHumidity?.Value,
                WindSpeed = p.WindSpeed,
                WindDirection = p.WindDirection,
                ShortForecast = p.ShortForecast,
                DetailedForecast = p.DetailedForecast,
                IconUrl = p.Icon
            }).ToList();

            return new ForecastResult
            {
                Location = location,
                GeneratedAt = forecastResponse.Properties.GeneratedAt,
                Periods = periods
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching forecast");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching forecast");
            return null;
        }
    }

    /// <summary>
    /// Gets radar information for the specified coordinates
    /// </summary>
    public async Task<RadarInfoResult?> GetRadarInfoAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Step 1: Get point metadata for nearest radar station
            var pointsResponse = await GetPointsDataAsync(latitude, longitude, cancellationToken);
            if (pointsResponse?.Properties is null)
            {
                _logger.LogWarning("No points data found for coordinates: {Lat}, {Lon}", latitude, longitude);
                return null;
            }

            var location = $"{pointsResponse.Properties.RelativeLocation?.Properties?.City}, {pointsResponse.Properties.RelativeLocation?.Properties?.State}";
            var radarStationId = pointsResponse.Properties.RadarStation;

            if (string.IsNullOrEmpty(radarStationId))
            {
                _logger.LogWarning("No radar station found");
                return null;
            }

            // Step 2: Get radar station details
            var radarStationUrl = $"radar/stations/{radarStationId}";

            NoaaRadarStationFeature? radarStation = null;
            try
            {
                radarStation = await _httpClient.GetFromJsonAsync<NoaaRadarStationFeature>(
                    radarStationUrl,
                    cancellationToken);
            }
            catch (HttpRequestException)
            {
                // Radar station details might not be available
                _logger.LogDebug("Could not fetch radar station details");
            }

            // Build radar URLs
            var radarImageUrl = $"https://radar.weather.gov/ridge/standard/{radarStationId}_loop.gif";
            var radarLoopUrl = $"https://radar.weather.gov/ridge/standard/{radarStationId}_0.gif";

            return new RadarInfoResult
            {
                Location = location,
                NearestRadarStation = radarStationId,
                RadarStationName = radarStation?.Properties?.Name ?? radarStationId,
                RadarStatus = radarStation?.Properties?.Rda?.Properties?.OperabilityStatus ?? "Unknown",
                RadarMode = radarStation?.Properties?.Rda?.Properties?.Mode ?? "Unknown",
                RadarLatitude = radarStation?.Geometry?.Coordinates?.ElementAtOrDefault(1),
                RadarLongitude = radarStation?.Geometry?.Coordinates?.ElementAtOrDefault(0),
                RadarImageUrl = radarLoopUrl,
                RadarLoopUrl = radarImageUrl
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching radar info");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching radar info");
            return null;
        }
    }

    private async Task<NoaaPointsResponse?> GetPointsDataAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken)
    {
        var pointsUrl = $"points/{latitude:F4},{longitude:F4}";
        _logger.LogDebug("Fetching NOAA points data from: {Url}", pointsUrl);

        return await _httpClient.GetFromJsonAsync<NoaaPointsResponse>(
            pointsUrl,
            cancellationToken);
    }

    #region Unit Conversion Helpers

    private static double? ConvertCelsiusToFahrenheit(double? celsius)
    {
        if (!celsius.HasValue) return null;
        return (celsius.Value * 9 / 5) + 32;
    }

    private static double? ConvertMetersPerSecondToMph(double? metersPerSecond)
    {
        if (!metersPerSecond.HasValue) return null;
        return metersPerSecond.Value * 2.237;
    }

    private static double? ConvertPascalsToInHg(double? pascals)
    {
        if (!pascals.HasValue) return null;
        return pascals.Value * 0.00029530;
    }

    private static double? ConvertMetersToMiles(double? meters)
    {
        if (!meters.HasValue) return null;
        return meters.Value * 0.000621371;
    }

    private static string? ConvertDegreesToCardinal(double? degrees)
    {
        if (!degrees.HasValue) return null;

        var directions = new[] { "N", "NNE", "NE", "ENE", "E", "ESE", "SE", "SSE",
                                  "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
        var index = (int)Math.Round(degrees.Value / 22.5) % 16;
        return directions[index];
    }

    #endregion
}


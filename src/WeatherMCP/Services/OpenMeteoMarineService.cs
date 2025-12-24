using WeatherMCP.Models;

namespace WeatherMCP.Services;

/// <summary>
/// Interface for Open Meteo Marine service operations
/// </summary>
public interface IOpenMeteoMarineService
{
    Task<MarineConditionsResult?> GetCurrentMarineConditionsAsync(double latitude, double longitude, CancellationToken cancellationToken = default);
    Task<MarineForecastResult?> GetMarineForecastAsync(double latitude, double longitude, int forecastDays = 7, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for interacting with Open Meteo Marine Weather API
/// </summary>
public sealed class OpenMeteoMarineService : IOpenMeteoMarineService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenMeteoMarineService> _logger;

    // All available current condition variables
    private static readonly string[] CurrentVariables =
    {
        "wave_height",
        "wave_direction",
        "wave_period",
        "wave_peak_period",
        "wind_wave_height",
        "wind_wave_direction",
        "wind_wave_period",
        "wind_wave_peak_period",
        "swell_wave_height",
        "swell_wave_direction",
        "swell_wave_period",
        "swell_wave_peak_period",
        "secondary_swell_wave_height",
        "secondary_swell_wave_direction",
        "secondary_swell_wave_period",
        "tertiary_swell_wave_height",
        "tertiary_swell_wave_direction",
        "tertiary_swell_wave_period",
        "sea_level_height_msl",
        "sea_surface_temperature",
        "ocean_current_velocity",
        "ocean_current_direction"
    };

    private static readonly string[] HourlyVariables =
    {
        "wave_height",
        "wave_direction",
        "wave_period",
        "wind_wave_height",
        "wind_wave_direction",
        "wind_wave_period",
        "swell_wave_height",
        "swell_wave_direction",
        "swell_wave_period",
        "sea_surface_temperature",
        "ocean_current_velocity",
        "ocean_current_direction"
    };

    private static readonly string[] DailyVariables =
    {
        "wave_height_max",
        "wave_direction_dominant",
        "wave_period_max",
        "wind_wave_height_max",
        "wind_wave_direction_dominant",
        "wind_wave_period_max",
        "swell_wave_height_max",
        "swell_wave_direction_dominant",
        "swell_wave_period_max"
    };

    public OpenMeteoMarineService(HttpClient httpClient, ILogger<OpenMeteoMarineService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Gets current marine conditions for the specified coordinates (all available variables)
    /// </summary>
    public async Task<MarineConditionsResult?> GetCurrentMarineConditionsAsync(
        double latitude,
        double longitude,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var currentParams = string.Join(",", CurrentVariables);
            var requestUrl = $"v1/marine?latitude={latitude:F4}&longitude={longitude:F4}&current={currentParams}&timezone=auto";

            _logger.LogDebug("Fetching marine conditions from: {Url}", requestUrl);

            var response = await _httpClient.GetFromJsonAsync<OpenMeteoMarineResponse>(
                requestUrl,
                cancellationToken);

            if (response?.Current is null)
            {
                _logger.LogWarning("No marine current data found for coordinates: {Lat}, {Lon}", latitude, longitude);
                return null;
            }

            var current = response.Current;

            return new MarineConditionsResult
            {
                Location = $"{latitude:F4}, {longitude:F4}",
                Latitude = response.Latitude,
                Longitude = response.Longitude,
                ObservationTime = DateTime.TryParse(current.Time, out var time) ? time : DateTime.UtcNow,
                Timezone = response.Timezone,

                // Wave information
                WaveHeightMeters = current.WaveHeight,
                WaveHeightFeet = ConvertMetersToFeet(current.WaveHeight),
                WaveDirectionDegrees = current.WaveDirection,
                WaveDirectionCardinal = ConvertDegreesToCardinal(current.WaveDirection),
                WavePeriodSeconds = current.WavePeriod,
                WavePeakPeriodSeconds = current.WavePeakPeriod,

                // Wind wave information
                WindWaveHeightMeters = current.WindWaveHeight,
                WindWaveHeightFeet = ConvertMetersToFeet(current.WindWaveHeight),
                WindWaveDirectionDegrees = current.WindWaveDirection,
                WindWaveDirectionCardinal = ConvertDegreesToCardinal(current.WindWaveDirection),
                WindWavePeriodSeconds = current.WindWavePeriod,
                WindWavePeakPeriodSeconds = current.WindWavePeakPeriod,

                // Swell information
                SwellWaveHeightMeters = current.SwellWaveHeight,
                SwellWaveHeightFeet = ConvertMetersToFeet(current.SwellWaveHeight),
                SwellWaveDirectionDegrees = current.SwellWaveDirection,
                SwellWaveDirectionCardinal = ConvertDegreesToCardinal(current.SwellWaveDirection),
                SwellWavePeriodSeconds = current.SwellWavePeriod,
                SwellWavePeakPeriodSeconds = current.SwellWavePeakPeriod,

                // Secondary swell
                SecondarySwellHeightMeters = current.SecondarySwellWaveHeight,
                SecondarySwellDirectionDegrees = current.SecondarySwellWaveDirection,
                SecondarySwellPeriodSeconds = current.SecondarySwellWavePeriod,

                // Tertiary swell
                TertiarySwellHeightMeters = current.TertiarySwellWaveHeight,
                TertiarySwellDirectionDegrees = current.TertiarySwellWaveDirection,
                TertiarySwellPeriodSeconds = current.TertiarySwellWavePeriod,

                // Sea conditions
                SeaLevelHeightMeters = current.SeaLevelHeightMsl,
                SeaSurfaceTemperatureC = current.SeaSurfaceTemperature,
                SeaSurfaceTemperatureF = ConvertCelsiusToFahrenheit(current.SeaSurfaceTemperature),

                // Ocean currents
                OceanCurrentVelocityKmh = current.OceanCurrentVelocity,
                OceanCurrentVelocityKnots = ConvertKmhToKnots(current.OceanCurrentVelocity),
                OceanCurrentDirectionDegrees = current.OceanCurrentDirection,
                OceanCurrentDirectionCardinal = ConvertDegreesToCardinal(current.OceanCurrentDirection)
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching marine conditions");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching marine conditions");
            return null;
        }
    }

    /// <summary>
    /// Gets marine forecast for the specified coordinates
    /// </summary>
    public async Task<MarineForecastResult?> GetMarineForecastAsync(
        double latitude,
        double longitude,
        int forecastDays = 7,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var hourlyParams = string.Join(",", HourlyVariables);
            var dailyParams = string.Join(",", DailyVariables);
            var requestUrl = $"v1/marine?latitude={latitude:F4}&longitude={longitude:F4}" +
                            $"&hourly={hourlyParams}&daily={dailyParams}" +
                            $"&forecast_days={forecastDays}&timezone=auto";

            _logger.LogDebug("Fetching marine forecast from: {Url}", requestUrl);

            var response = await _httpClient.GetFromJsonAsync<OpenMeteoMarineResponse>(
                requestUrl,
                cancellationToken);

            if (response is null)
            {
                _logger.LogWarning("No marine forecast data found for coordinates: {Lat}, {Lon}", latitude, longitude);
                return null;
            }

            var result = new MarineForecastResult
            {
                Location = $"{latitude:F4}, {longitude:F4}",
                Latitude = response.Latitude,
                Longitude = response.Longitude,
                GeneratedAt = DateTime.UtcNow,
                Timezone = response.Timezone,
                HourlyForecast = [],
                DailyForecast = []
            };

            // Process hourly data
            if (response.Hourly?.Time is not null)
            {
                for (int i = 0; i < response.Hourly.Time.Count; i++)
                {
                    result.HourlyForecast.Add(new MarineHourlyForecast
                    {
                        Time = DateTime.TryParse(response.Hourly.Time[i], out var time) ? time : DateTime.UtcNow,
                        WaveHeightMeters = GetValueAtIndex(response.Hourly.WaveHeight, i),
                        WaveDirectionDegrees = GetValueAtIndex(response.Hourly.WaveDirection, i),
                        WavePeriodSeconds = GetValueAtIndex(response.Hourly.WavePeriod, i),
                        WindWaveHeightMeters = GetValueAtIndex(response.Hourly.WindWaveHeight, i),
                        SwellWaveHeightMeters = GetValueAtIndex(response.Hourly.SwellWaveHeight, i),
                        SeaSurfaceTemperatureC = GetValueAtIndex(response.Hourly.SeaSurfaceTemperature, i),
                        OceanCurrentVelocityKmh = GetValueAtIndex(response.Hourly.OceanCurrentVelocity, i),
                        OceanCurrentDirectionDegrees = GetValueAtIndex(response.Hourly.OceanCurrentDirection, i)
                    });
                }
            }

            // Process daily data
            if (response.Daily?.Time is not null)
            {
                for (int i = 0; i < response.Daily.Time.Count; i++)
                {
                    result.DailyForecast.Add(new MarineDailyForecast
                    {
                        Date = DateTime.TryParse(response.Daily.Time[i], out var date) ? date : DateTime.UtcNow,
                        WaveHeightMaxMeters = GetValueAtIndex(response.Daily.WaveHeightMax, i),
                        WaveDirectionDominantDegrees = GetValueAtIndex(response.Daily.WaveDirectionDominant, i),
                        WavePeriodMaxSeconds = GetValueAtIndex(response.Daily.WavePeriodMax, i),
                        WindWaveHeightMaxMeters = GetValueAtIndex(response.Daily.WindWaveHeightMax, i),
                        SwellWaveHeightMaxMeters = GetValueAtIndex(response.Daily.SwellWaveHeightMax, i)
                    });
                }
            }

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error fetching marine forecast");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching marine forecast");
            return null;
        }
    }

    #region Helper Methods

    private static double? GetValueAtIndex(List<double?>? list, int index)
    {
        if (list is null || index < 0 || index >= list.Count)
            return null;
        return list[index];
    }

    private static double? ConvertMetersToFeet(double? meters)
    {
        if (!meters.HasValue) return null;
        return meters.Value * 3.28084;
    }

    private static double? ConvertCelsiusToFahrenheit(double? celsius)
    {
        if (!celsius.HasValue) return null;
        return (celsius.Value * 9 / 5) + 32;
    }

    private static double? ConvertKmhToKnots(double? kmh)
    {
        if (!kmh.HasValue) return null;
        return kmh.Value * 0.539957;
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

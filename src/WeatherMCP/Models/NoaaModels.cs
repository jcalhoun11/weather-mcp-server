using System.Text.Json.Serialization;

namespace WeatherMCP.Models;

/// <summary>
/// NOAA Points API response for grid coordinates
/// </summary>
public sealed class NoaaPointsResponse
{
    [JsonPropertyName("properties")]
    public NoaaPointsProperties? Properties { get; set; }
}

public sealed class NoaaPointsProperties
{
    [JsonPropertyName("gridId")]
    public string? GridId { get; set; }

    [JsonPropertyName("gridX")]
    public int GridX { get; set; }

    [JsonPropertyName("gridY")]
    public int GridY { get; set; }

    [JsonPropertyName("forecast")]
    public string? ForecastUrl { get; set; }

    [JsonPropertyName("forecastHourly")]
    public string? ForecastHourlyUrl { get; set; }

    [JsonPropertyName("forecastGridData")]
    public string? ForecastGridDataUrl { get; set; }

    [JsonPropertyName("observationStations")]
    public string? ObservationStationsUrl { get; set; }

    [JsonPropertyName("relativeLocation")]
    public NoaaRelativeLocation? RelativeLocation { get; set; }

    [JsonPropertyName("radarStation")]
    public string? RadarStation { get; set; }
}

public sealed class NoaaRelativeLocation
{
    [JsonPropertyName("properties")]
    public NoaaRelativeLocationProperties? Properties { get; set; }
}

public sealed class NoaaRelativeLocationProperties
{
    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }
}

/// <summary>
/// NOAA Forecast API response
/// </summary>
public sealed class NoaaForecastResponse
{
    [JsonPropertyName("properties")]
    public NoaaForecastProperties? Properties { get; set; }
}

public sealed class NoaaForecastProperties
{
    [JsonPropertyName("updated")]
    public DateTime Updated { get; set; }

    [JsonPropertyName("generatedAt")]
    public DateTime GeneratedAt { get; set; }

    [JsonPropertyName("periods")]
    public List<NoaaForecastPeriod>? Periods { get; set; }
}

public sealed class NoaaForecastPeriod
{
    [JsonPropertyName("number")]
    public int Number { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("startTime")]
    public DateTime StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime EndTime { get; set; }

    [JsonPropertyName("isDaytime")]
    public bool IsDaytime { get; set; }

    [JsonPropertyName("temperature")]
    public int Temperature { get; set; }

    [JsonPropertyName("temperatureUnit")]
    public string? TemperatureUnit { get; set; }

    [JsonPropertyName("temperatureTrend")]
    public string? TemperatureTrend { get; set; }

    [JsonPropertyName("probabilityOfPrecipitation")]
    public NoaaProbability? ProbabilityOfPrecipitation { get; set; }

    [JsonPropertyName("relativeHumidity")]
    public NoaaProbability? RelativeHumidity { get; set; }

    [JsonPropertyName("windSpeed")]
    public string? WindSpeed { get; set; }

    [JsonPropertyName("windDirection")]
    public string? WindDirection { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("shortForecast")]
    public string? ShortForecast { get; set; }

    [JsonPropertyName("detailedForecast")]
    public string? DetailedForecast { get; set; }
}

public sealed class NoaaProbability
{
    [JsonPropertyName("unitCode")]
    public string? UnitCode { get; set; }

    [JsonPropertyName("value")]
    public double? Value { get; set; }
}

/// <summary>
/// NOAA Observation Stations response
/// </summary>
public sealed class NoaaStationsResponse
{
    [JsonPropertyName("features")]
    public List<NoaaStationFeature>? Features { get; set; }
}

public sealed class NoaaStationFeature
{
    [JsonPropertyName("properties")]
    public NoaaStationProperties? Properties { get; set; }
}

public sealed class NoaaStationProperties
{
    [JsonPropertyName("stationIdentifier")]
    public string? StationIdentifier { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

/// <summary>
/// NOAA Observation response for current conditions
/// </summary>
public sealed class NoaaObservationResponse
{
    [JsonPropertyName("properties")]
    public NoaaObservationProperties? Properties { get; set; }
}

public sealed class NoaaObservationProperties
{
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonPropertyName("textDescription")]
    public string? TextDescription { get; set; }

    [JsonPropertyName("icon")]
    public string? Icon { get; set; }

    [JsonPropertyName("temperature")]
    public NoaaMeasurement? Temperature { get; set; }

    [JsonPropertyName("dewpoint")]
    public NoaaMeasurement? Dewpoint { get; set; }

    [JsonPropertyName("windDirection")]
    public NoaaMeasurement? WindDirection { get; set; }

    [JsonPropertyName("windSpeed")]
    public NoaaMeasurement? WindSpeed { get; set; }

    [JsonPropertyName("windGust")]
    public NoaaMeasurement? WindGust { get; set; }

    [JsonPropertyName("barometricPressure")]
    public NoaaMeasurement? BarometricPressure { get; set; }

    [JsonPropertyName("seaLevelPressure")]
    public NoaaMeasurement? SeaLevelPressure { get; set; }

    [JsonPropertyName("visibility")]
    public NoaaMeasurement? Visibility { get; set; }

    [JsonPropertyName("relativeHumidity")]
    public NoaaMeasurement? RelativeHumidity { get; set; }

    [JsonPropertyName("windChill")]
    public NoaaMeasurement? WindChill { get; set; }

    [JsonPropertyName("heatIndex")]
    public NoaaMeasurement? HeatIndex { get; set; }
}

public sealed class NoaaMeasurement
{
    [JsonPropertyName("unitCode")]
    public string? UnitCode { get; set; }

    [JsonPropertyName("value")]
    public double? Value { get; set; }

    [JsonPropertyName("qualityControl")]
    public string? QualityControl { get; set; }
}

/// <summary>
/// NOAA Radar Stations response
/// </summary>
public sealed class NoaaRadarStationsResponse
{
    [JsonPropertyName("features")]
    public List<NoaaRadarStationFeature>? Features { get; set; }
}

public sealed class NoaaRadarStationFeature
{
    [JsonPropertyName("properties")]
    public NoaaRadarStationProperties? Properties { get; set; }

    [JsonPropertyName("geometry")]
    public NoaaGeometry? Geometry { get; set; }
}

public sealed class NoaaRadarStationProperties
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("stationType")]
    public string? StationType { get; set; }

    [JsonPropertyName("rda")]
    public NoaaRadarRda? Rda { get; set; }
}

public sealed class NoaaRadarRda
{
    [JsonPropertyName("properties")]
    public NoaaRadarRdaProperties? Properties { get; set; }
}

public sealed class NoaaRadarRdaProperties
{
    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("operabilityStatus")]
    public string? OperabilityStatus { get; set; }

    [JsonPropertyName("superResolutionStatus")]
    public string? SuperResolutionStatus { get; set; }
}

public sealed class NoaaGeometry
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("coordinates")]
    public List<double>? Coordinates { get; set; }
}

/// <summary>
/// Simplified weather result for API responses
/// </summary>
public sealed class CurrentConditionsResult
{
    public string? Location { get; set; }
    public DateTime ObservationTime { get; set; }
    public string? Description { get; set; }
    public double? TemperatureF { get; set; }
    public double? TemperatureC { get; set; }
    public double? FeelsLikeF { get; set; }
    public double? FeelsLikeC { get; set; }
    public double? Humidity { get; set; }
    public double? DewpointF { get; set; }
    public double? DewpointC { get; set; }
    public string? WindSpeed { get; set; }
    public string? WindDirection { get; set; }
    public double? WindGustMph { get; set; }
    public double? BarometricPressureInHg { get; set; }
    public double? VisibilityMiles { get; set; }
    public string? IconUrl { get; set; }
}

public sealed class ForecastResult
{
    public string? Location { get; set; }
    public DateTime GeneratedAt { get; set; }
    public List<ForecastPeriodResult>? Periods { get; set; }
}

public sealed class ForecastPeriodResult
{
    public string? Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsDaytime { get; set; }
    public int Temperature { get; set; }
    public string? TemperatureUnit { get; set; }
    public int? PrecipitationChance { get; set; }
    public int? Humidity { get; set; }
    public string? WindSpeed { get; set; }
    public string? WindDirection { get; set; }
    public string? ShortForecast { get; set; }
    public string? DetailedForecast { get; set; }
    public string? IconUrl { get; set; }
}

public sealed class RadarInfoResult
{
    public string? Location { get; set; }
    public string? NearestRadarStation { get; set; }
    public string? RadarStationName { get; set; }
    public string? RadarStatus { get; set; }
    public string? RadarMode { get; set; }
    public double? RadarLatitude { get; set; }
    public double? RadarLongitude { get; set; }
    public string? RadarImageUrl { get; set; }
    public string? RadarLoopUrl { get; set; }
}

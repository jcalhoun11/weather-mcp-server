using System.Text.Json.Serialization;

namespace WeatherMCP.Models;

/// <summary>
/// Open Meteo Marine API response
/// </summary>
public sealed class OpenMeteoMarineResponse
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public double GenerationTimeMs { get; set; }

    [JsonPropertyName("utc_offset_seconds")]
    public int UtcOffsetSeconds { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("timezone_abbreviation")]
    public string? TimezoneAbbreviation { get; set; }

    [JsonPropertyName("current")]
    public OpenMeteoMarineCurrent? Current { get; set; }

    [JsonPropertyName("current_units")]
    public OpenMeteoMarineCurrentUnits? CurrentUnits { get; set; }

    [JsonPropertyName("hourly")]
    public OpenMeteoMarineHourly? Hourly { get; set; }

    [JsonPropertyName("hourly_units")]
    public OpenMeteoMarineHourlyUnits? HourlyUnits { get; set; }

    [JsonPropertyName("daily")]
    public OpenMeteoMarineDaily? Daily { get; set; }

    [JsonPropertyName("daily_units")]
    public OpenMeteoMarineDailyUnits? DailyUnits { get; set; }
}

/// <summary>
/// Current marine conditions
/// </summary>
public sealed class OpenMeteoMarineCurrent
{
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [JsonPropertyName("interval")]
    public int Interval { get; set; }

    [JsonPropertyName("wave_height")]
    public double? WaveHeight { get; set; }

    [JsonPropertyName("wave_direction")]
    public double? WaveDirection { get; set; }

    [JsonPropertyName("wave_period")]
    public double? WavePeriod { get; set; }

    [JsonPropertyName("wave_peak_period")]
    public double? WavePeakPeriod { get; set; }

    [JsonPropertyName("wind_wave_height")]
    public double? WindWaveHeight { get; set; }

    [JsonPropertyName("wind_wave_direction")]
    public double? WindWaveDirection { get; set; }

    [JsonPropertyName("wind_wave_period")]
    public double? WindWavePeriod { get; set; }

    [JsonPropertyName("wind_wave_peak_period")]
    public double? WindWavePeakPeriod { get; set; }

    [JsonPropertyName("swell_wave_height")]
    public double? SwellWaveHeight { get; set; }

    [JsonPropertyName("swell_wave_direction")]
    public double? SwellWaveDirection { get; set; }

    [JsonPropertyName("swell_wave_period")]
    public double? SwellWavePeriod { get; set; }

    [JsonPropertyName("swell_wave_peak_period")]
    public double? SwellWavePeakPeriod { get; set; }

    [JsonPropertyName("secondary_swell_wave_height")]
    public double? SecondarySwellWaveHeight { get; set; }

    [JsonPropertyName("secondary_swell_wave_direction")]
    public double? SecondarySwellWaveDirection { get; set; }

    [JsonPropertyName("secondary_swell_wave_period")]
    public double? SecondarySwellWavePeriod { get; set; }

    [JsonPropertyName("tertiary_swell_wave_height")]
    public double? TertiarySwellWaveHeight { get; set; }

    [JsonPropertyName("tertiary_swell_wave_direction")]
    public double? TertiarySwellWaveDirection { get; set; }

    [JsonPropertyName("tertiary_swell_wave_period")]
    public double? TertiarySwellWavePeriod { get; set; }

    [JsonPropertyName("sea_level_height_msl")]
    public double? SeaLevelHeightMsl { get; set; }

    [JsonPropertyName("sea_surface_temperature")]
    public double? SeaSurfaceTemperature { get; set; }

    [JsonPropertyName("ocean_current_velocity")]
    public double? OceanCurrentVelocity { get; set; }

    [JsonPropertyName("ocean_current_direction")]
    public double? OceanCurrentDirection { get; set; }
}

/// <summary>
/// Units for current marine conditions
/// </summary>
public sealed class OpenMeteoMarineCurrentUnits
{
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [JsonPropertyName("interval")]
    public string? Interval { get; set; }

    [JsonPropertyName("wave_height")]
    public string? WaveHeight { get; set; }

    [JsonPropertyName("wave_direction")]
    public string? WaveDirection { get; set; }

    [JsonPropertyName("wave_period")]
    public string? WavePeriod { get; set; }

    [JsonPropertyName("wave_peak_period")]
    public string? WavePeakPeriod { get; set; }

    [JsonPropertyName("wind_wave_height")]
    public string? WindWaveHeight { get; set; }

    [JsonPropertyName("wind_wave_direction")]
    public string? WindWaveDirection { get; set; }

    [JsonPropertyName("wind_wave_period")]
    public string? WindWavePeriod { get; set; }

    [JsonPropertyName("swell_wave_height")]
    public string? SwellWaveHeight { get; set; }

    [JsonPropertyName("swell_wave_direction")]
    public string? SwellWaveDirection { get; set; }

    [JsonPropertyName("swell_wave_period")]
    public string? SwellWavePeriod { get; set; }

    [JsonPropertyName("sea_surface_temperature")]
    public string? SeaSurfaceTemperature { get; set; }

    [JsonPropertyName("ocean_current_velocity")]
    public string? OceanCurrentVelocity { get; set; }

    [JsonPropertyName("ocean_current_direction")]
    public string? OceanCurrentDirection { get; set; }
}

/// <summary>
/// Hourly marine data
/// </summary>
public sealed class OpenMeteoMarineHourly
{
    [JsonPropertyName("time")]
    public List<string>? Time { get; set; }

    [JsonPropertyName("wave_height")]
    public List<double?>? WaveHeight { get; set; }

    [JsonPropertyName("wave_direction")]
    public List<double?>? WaveDirection { get; set; }

    [JsonPropertyName("wave_period")]
    public List<double?>? WavePeriod { get; set; }

    [JsonPropertyName("wind_wave_height")]
    public List<double?>? WindWaveHeight { get; set; }

    [JsonPropertyName("wind_wave_direction")]
    public List<double?>? WindWaveDirection { get; set; }

    [JsonPropertyName("wind_wave_period")]
    public List<double?>? WindWavePeriod { get; set; }

    [JsonPropertyName("swell_wave_height")]
    public List<double?>? SwellWaveHeight { get; set; }

    [JsonPropertyName("swell_wave_direction")]
    public List<double?>? SwellWaveDirection { get; set; }

    [JsonPropertyName("swell_wave_period")]
    public List<double?>? SwellWavePeriod { get; set; }

    [JsonPropertyName("sea_surface_temperature")]
    public List<double?>? SeaSurfaceTemperature { get; set; }

    [JsonPropertyName("ocean_current_velocity")]
    public List<double?>? OceanCurrentVelocity { get; set; }

    [JsonPropertyName("ocean_current_direction")]
    public List<double?>? OceanCurrentDirection { get; set; }
}

/// <summary>
/// Units for hourly marine data
/// </summary>
public sealed class OpenMeteoMarineHourlyUnits
{
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [JsonPropertyName("wave_height")]
    public string? WaveHeight { get; set; }

    [JsonPropertyName("wave_direction")]
    public string? WaveDirection { get; set; }

    [JsonPropertyName("wave_period")]
    public string? WavePeriod { get; set; }

    [JsonPropertyName("wind_wave_height")]
    public string? WindWaveHeight { get; set; }

    [JsonPropertyName("wind_wave_direction")]
    public string? WindWaveDirection { get; set; }

    [JsonPropertyName("wind_wave_period")]
    public string? WindWavePeriod { get; set; }

    [JsonPropertyName("swell_wave_height")]
    public string? SwellWaveHeight { get; set; }

    [JsonPropertyName("swell_wave_direction")]
    public string? SwellWaveDirection { get; set; }

    [JsonPropertyName("swell_wave_period")]
    public string? SwellWavePeriod { get; set; }

    [JsonPropertyName("sea_surface_temperature")]
    public string? SeaSurfaceTemperature { get; set; }

    [JsonPropertyName("ocean_current_velocity")]
    public string? OceanCurrentVelocity { get; set; }

    [JsonPropertyName("ocean_current_direction")]
    public string? OceanCurrentDirection { get; set; }
}

/// <summary>
/// Daily marine data
/// </summary>
public sealed class OpenMeteoMarineDaily
{
    [JsonPropertyName("time")]
    public List<string>? Time { get; set; }

    [JsonPropertyName("wave_height_max")]
    public List<double?>? WaveHeightMax { get; set; }

    [JsonPropertyName("wave_direction_dominant")]
    public List<double?>? WaveDirectionDominant { get; set; }

    [JsonPropertyName("wave_period_max")]
    public List<double?>? WavePeriodMax { get; set; }

    [JsonPropertyName("wind_wave_height_max")]
    public List<double?>? WindWaveHeightMax { get; set; }

    [JsonPropertyName("wind_wave_direction_dominant")]
    public List<double?>? WindWaveDirectionDominant { get; set; }

    [JsonPropertyName("wind_wave_period_max")]
    public List<double?>? WindWavePeriodMax { get; set; }

    [JsonPropertyName("swell_wave_height_max")]
    public List<double?>? SwellWaveHeightMax { get; set; }

    [JsonPropertyName("swell_wave_direction_dominant")]
    public List<double?>? SwellWaveDirectionDominant { get; set; }

    [JsonPropertyName("swell_wave_period_max")]
    public List<double?>? SwellWavePeriodMax { get; set; }
}

/// <summary>
/// Units for daily marine data
/// </summary>
public sealed class OpenMeteoMarineDailyUnits
{
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [JsonPropertyName("wave_height_max")]
    public string? WaveHeightMax { get; set; }

    [JsonPropertyName("wave_direction_dominant")]
    public string? WaveDirectionDominant { get; set; }

    [JsonPropertyName("wave_period_max")]
    public string? WavePeriodMax { get; set; }

    [JsonPropertyName("wind_wave_height_max")]
    public string? WindWaveHeightMax { get; set; }

    [JsonPropertyName("wind_wave_direction_dominant")]
    public string? WindWaveDirectionDominant { get; set; }

    [JsonPropertyName("wind_wave_period_max")]
    public string? WindWavePeriodMax { get; set; }

    [JsonPropertyName("swell_wave_height_max")]
    public string? SwellWaveHeightMax { get; set; }

    [JsonPropertyName("swell_wave_direction_dominant")]
    public string? SwellWaveDirectionDominant { get; set; }

    [JsonPropertyName("swell_wave_period_max")]
    public string? SwellWavePeriodMax { get; set; }
}

/// <summary>
/// Simplified marine conditions result for API responses
/// </summary>
public sealed class MarineConditionsResult
{
    public string? Location { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime ObservationTime { get; set; }
    public string? Timezone { get; set; }

    // Wave information
    public double? WaveHeightMeters { get; set; }
    public double? WaveHeightFeet { get; set; }
    public double? WaveDirectionDegrees { get; set; }
    public string? WaveDirectionCardinal { get; set; }
    public double? WavePeriodSeconds { get; set; }
    public double? WavePeakPeriodSeconds { get; set; }

    // Wind wave information
    public double? WindWaveHeightMeters { get; set; }
    public double? WindWaveHeightFeet { get; set; }
    public double? WindWaveDirectionDegrees { get; set; }
    public string? WindWaveDirectionCardinal { get; set; }
    public double? WindWavePeriodSeconds { get; set; }
    public double? WindWavePeakPeriodSeconds { get; set; }

    // Swell information
    public double? SwellWaveHeightMeters { get; set; }
    public double? SwellWaveHeightFeet { get; set; }
    public double? SwellWaveDirectionDegrees { get; set; }
    public string? SwellWaveDirectionCardinal { get; set; }
    public double? SwellWavePeriodSeconds { get; set; }
    public double? SwellWavePeakPeriodSeconds { get; set; }

    // Secondary swell
    public double? SecondarySwellHeightMeters { get; set; }
    public double? SecondarySwellDirectionDegrees { get; set; }
    public double? SecondarySwellPeriodSeconds { get; set; }

    // Tertiary swell
    public double? TertiarySwellHeightMeters { get; set; }
    public double? TertiarySwellDirectionDegrees { get; set; }
    public double? TertiarySwellPeriodSeconds { get; set; }

    // Sea conditions
    public double? SeaLevelHeightMeters { get; set; }
    public double? SeaSurfaceTemperatureC { get; set; }
    public double? SeaSurfaceTemperatureF { get; set; }

    // Ocean currents
    public double? OceanCurrentVelocityKmh { get; set; }
    public double? OceanCurrentVelocityKnots { get; set; }
    public double? OceanCurrentDirectionDegrees { get; set; }
    public string? OceanCurrentDirectionCardinal { get; set; }
}

/// <summary>
/// Marine forecast result with hourly and daily data
/// </summary>
public sealed class MarineForecastResult
{
    public string? Location { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime GeneratedAt { get; set; }
    public string? Timezone { get; set; }
    public List<MarineHourlyForecast>? HourlyForecast { get; set; }
    public List<MarineDailyForecast>? DailyForecast { get; set; }
}

public sealed class MarineHourlyForecast
{
    public DateTime Time { get; set; }
    public double? WaveHeightMeters { get; set; }
    public double? WaveDirectionDegrees { get; set; }
    public double? WavePeriodSeconds { get; set; }
    public double? WindWaveHeightMeters { get; set; }
    public double? SwellWaveHeightMeters { get; set; }
    public double? SeaSurfaceTemperatureC { get; set; }
    public double? OceanCurrentVelocityKmh { get; set; }
    public double? OceanCurrentDirectionDegrees { get; set; }
}

public sealed class MarineDailyForecast
{
    public DateTime Date { get; set; }
    public double? WaveHeightMaxMeters { get; set; }
    public double? WaveDirectionDominantDegrees { get; set; }
    public double? WavePeriodMaxSeconds { get; set; }
    public double? WindWaveHeightMaxMeters { get; set; }
    public double? SwellWaveHeightMaxMeters { get; set; }
}

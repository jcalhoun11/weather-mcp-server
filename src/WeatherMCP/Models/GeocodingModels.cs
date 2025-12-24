using System.Text.Json.Serialization;

namespace WeatherMCP.Models;

/// <summary>
/// Geocoding response from Open-Meteo Geocoding API
/// </summary>
public sealed class GeocodingResponse
{
    [JsonPropertyName("results")]
    public List<GeocodingResult>? Results { get; set; }

    [JsonPropertyName("generationtime_ms")]
    public double GenerationTimeMs { get; set; }
}

public sealed class GeocodingResult
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("elevation")]
    public double? Elevation { get; set; }

    [JsonPropertyName("feature_code")]
    public string? FeatureCode { get; set; }

    [JsonPropertyName("country_code")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("admin1_id")]
    public long? Admin1Id { get; set; }

    [JsonPropertyName("admin2_id")]
    public long? Admin2Id { get; set; }

    [JsonPropertyName("admin3_id")]
    public long? Admin3Id { get; set; }

    [JsonPropertyName("admin4_id")]
    public long? Admin4Id { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("population")]
    public int? Population { get; set; }

    [JsonPropertyName("postcodes")]
    public List<string>? Postcodes { get; set; }

    [JsonPropertyName("country_id")]
    public long? CountryId { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

    [JsonPropertyName("admin1")]
    public string? Admin1 { get; set; }

    [JsonPropertyName("admin2")]
    public string? Admin2 { get; set; }

    [JsonPropertyName("admin3")]
    public string? Admin3 { get; set; }

    [JsonPropertyName("admin4")]
    public string? Admin4 { get; set; }
}

/// <summary>
/// Represents a geographic location with coordinates
/// </summary>
public sealed class GeoLocation
{
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? Timezone { get; set; }

    public string DisplayName => string.IsNullOrWhiteSpace(State)
        ? $"{City}, {Country}"
        : $"{City}, {State}, {Country}";
}

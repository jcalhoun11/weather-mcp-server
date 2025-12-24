using System.Text.RegularExpressions;
using WeatherMCP.Models;

namespace WeatherMCP.Services;

/// <summary>
/// Interface for geocoding operations
/// </summary>
public interface IGeocodingService
{
    Task<GeoLocation?> GetCoordinatesAsync(string location, CancellationToken cancellationToken = default);
    Task<GeoLocation?> GetCoordinatesFromZipCodeAsync(string zipCode, CancellationToken cancellationToken = default);
}

/// <summary>
/// Service for geocoding locations using Open-Meteo Geocoding API
/// </summary>
public sealed class GeocodingService : IGeocodingService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GeocodingService> _logger;

    // Common US zip code pattern
    private static readonly Regex UsZipCodePattern = new(@"^\d{5}(-\d{4})?$", RegexOptions.Compiled);

    public GeocodingService(HttpClient httpClient, ILogger<GeocodingService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    /// <summary>
    /// Gets coordinates for a city name or location string
    /// </summary>
    public async Task<GeoLocation?> GetCoordinatesAsync(string location, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(location))
        {
            _logger.LogWarning("Empty location provided for geocoding");
            return null;
        }

        // Check if it's a US zip code
        if (UsZipCodePattern.IsMatch(location.Trim()))
        {
            return await GetCoordinatesFromZipCodeAsync(location.Trim(), cancellationToken);
        }

        try
        {
            var encodedLocation = Uri.EscapeDataString(location);
            var requestUrl = $"v1/search?name={encodedLocation}&count=1&language=en&format=json";

            _logger.LogDebug("Geocoding request for location: {Location}", location);

            var response = await _httpClient.GetFromJsonAsync<GeocodingResponse>(
                requestUrl,
                cancellationToken);

            if (response?.Results is null || response.Results.Count == 0)
            {
                _logger.LogWarning("No geocoding results found for location: {Location}", location);
                return null;
            }

            var result = response.Results[0];

            return new GeoLocation
            {
                City = result.Name,
                State = result.Admin1,
                Country = result.Country,
                Latitude = result.Latitude,
                Longitude = result.Longitude,
                Timezone = result.Timezone
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error during geocoding for location: {Location}", location);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during geocoding for location: {Location}", location);
            return null;
        }
    }

    /// <summary>
    /// Gets coordinates for a US zip code
    /// </summary>
    public async Task<GeoLocation?> GetCoordinatesFromZipCodeAsync(string zipCode, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(zipCode))
        {
            _logger.LogWarning("Empty zip code provided for geocoding");
            return null;
        }

        // Extract just the 5-digit portion
        var zip5 = zipCode.Trim().Split('-')[0];

        try
        {
            // Use postal code search
            var requestUrl = $"v1/search?name={zip5}&count=5&language=en&format=json";

            _logger.LogDebug("Geocoding request for zip code: {ZipCode}", zipCode);

            var response = await _httpClient.GetFromJsonAsync<GeocodingResponse>(
                requestUrl,
                cancellationToken);

            if (response?.Results is null || response.Results.Count == 0)
            {
                _logger.LogWarning("No geocoding results found for zip code: {ZipCode}", zipCode);
                return null;
            }

            // Find the result that matches the zip code in postcodes
            var result = response.Results
                .FirstOrDefault(r => r.Postcodes?.Contains(zip5) == true)
                ?? response.Results[0];

            return new GeoLocation
            {
                City = result.Name,
                State = result.Admin1,
                Country = result.Country,
                Latitude = result.Latitude,
                Longitude = result.Longitude,
                Timezone = result.Timezone
            };
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error during geocoding for zip code: {ZipCode}", zipCode);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during geocoding for zip code: {ZipCode}", zipCode);
            return null;
        }
    }
}


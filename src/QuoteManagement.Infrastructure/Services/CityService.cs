using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using QuoteManagement.Application.Interfaces;

namespace QuoteManagement.Infrastructure.Services;

public class CityService : ICityService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<CityService> _logger;

    public CityService(HttpClient httpClient, IConfiguration configuration, ILogger<CityService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["ApiNinjas:ApiKey"] ?? throw new InvalidOperationException("API Ninjas API Key not configured");
        _logger = logger;
    }

    public async Task<IEnumerable<string>> GetCitiesByCountryAsync(string countryCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(countryCode))
            {
                _logger.LogWarning("Country code is empty");
                return Enumerable.Empty<string>();
            }

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://api.api-ninjas.com/v1/city?country={countryCode.ToUpper()}");
            request.Headers.Add("X-Api-Key", _apiKey);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("API Ninjas error: {StatusCode} for country code: {CountryCode}",
                    response.StatusCode, countryCode);
                return Enumerable.Empty<string>();
            }

            var content = await response.Content.ReadAsStringAsync();
            var cities = JsonSerializer.Deserialize<List<CityResponse>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return cities?.Select(c => c.Name).Distinct().OrderBy(c => c) ?? Enumerable.Empty<string>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching cities for country code: {CountryCode}", countryCode);
            return Enumerable.Empty<string>();
        }
    }

    private class CityResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}

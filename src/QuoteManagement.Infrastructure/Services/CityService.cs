using System.Text.Json;
using Microsoft.Extensions.Configuration;
using QuoteManagement.Application.Interfaces;

namespace QuoteManagement.Infrastructure.Services;

public class CityService : ICityService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public CityService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["ApiNinjas:ApiKey"] ?? throw new InvalidOperationException("API Ninjas API Key not configured");
    }

    public async Task<IEnumerable<string>> GetCitiesByCountryAsync(string country)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, 
                $"https://api.api-ninjas.com/v1/city?country={Uri.EscapeDataString(country)}&limit=30");
            request.Headers.Add("X-Api-Key", _apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var cities = JsonSerializer.Deserialize<List<CityResponse>>(content, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            return cities?.Select(c => c.Name).Distinct() ?? Enumerable.Empty<string>();
        }
        catch (Exception ex)
        {
            // Log the exception in production
            Console.WriteLine($"Error fetching cities: {ex.Message}");
            return Enumerable.Empty<string>();
        }
    }

    private class CityResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}

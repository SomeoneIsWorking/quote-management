using System.Text.Json;
using Microsoft.Extensions.Configuration;
using QuoteManagement.Application.Interfaces;

namespace QuoteManagement.Infrastructure.Services;

public class CurrencyService : ICurrencyService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public CurrencyService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["ApiNinjas:ApiKey"] ?? throw new InvalidOperationException("API Ninjas API Key not configured");
    }

    public async Task<decimal> ConvertToUsdAsync(string fromCurrency, decimal amount)
    {
        // If already USD, return the same amount
        if (fromCurrency.Equals("USD", StringComparison.OrdinalIgnoreCase))
        {
            return amount;
        }

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, 
                $"https://api.api-ninjas.com/v1/convertcurrency?want=USD&have={fromCurrency.ToUpper()}&amount={amount}");
            request.Headers.Add("X-Api-Key", _apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<CurrencyConversionResponse>(content, new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            });

            return result?.NewAmount ?? amount;
        }
        catch (Exception ex)
        {
            // Log the exception in production
            Console.WriteLine($"Error converting currency: {ex.Message}");
            return amount; // Return original amount if conversion fails
        }
    }

    private class CurrencyConversionResponse
    {
        public decimal NewAmount { get; set; }
        public string NewCurrency { get; set; } = string.Empty;
        public decimal OldAmount { get; set; }
        public string OldCurrency { get; set; } = string.Empty;
    }
}

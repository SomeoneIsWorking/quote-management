namespace QuoteManagement.Application.Interfaces;

public interface ICityService
{
    Task<IEnumerable<string>> GetCitiesByCountryAsync(string country);
}

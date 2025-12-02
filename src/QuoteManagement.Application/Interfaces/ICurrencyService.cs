namespace QuoteManagement.Application.Interfaces;

public interface ICurrencyService
{
    Task<decimal> ConvertToUsdAsync(string fromCurrency, decimal amount);
}

using QuoteManagement.Domain.Entities;

namespace QuoteManagement.Domain.Interfaces;

public interface IQuoteRepository
{
    Task<QuoteRequest> CreateAsync(QuoteRequest quoteRequest);
    Task<IEnumerable<QuoteRequest>> GetAllAsync();
    Task<QuoteRequest?> GetByIdAsync(Guid id);
}

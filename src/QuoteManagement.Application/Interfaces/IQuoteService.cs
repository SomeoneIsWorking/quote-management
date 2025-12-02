using QuoteManagement.Application.DTOs;

namespace QuoteManagement.Application.Interfaces;

public interface IQuoteService
{
    Task<QuoteResponseDto> CreateQuoteAsync(QuoteRequestDto request, Guid userId);
    Task<IEnumerable<QuoteResponseDto>> GetAllQuotesAsync();
}

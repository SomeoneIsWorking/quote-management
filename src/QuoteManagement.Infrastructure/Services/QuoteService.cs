using QuoteManagement.Application.DTOs;
using QuoteManagement.Application.Interfaces;
using QuoteManagement.Domain.Entities;
using QuoteManagement.Domain.Interfaces;

namespace QuoteManagement.Infrastructure.Services;

public class QuoteService : IQuoteService
{
    private readonly IQuoteRepository _quoteRepository;
    private readonly ICurrencyService _currencyService;
    private readonly IUserRepository _userRepository;

    public QuoteService(
        IQuoteRepository quoteRepository, 
        ICurrencyService currencyService,
        IUserRepository userRepository)
    {
        _quoteRepository = quoteRepository;
        _currencyService = currencyService;
        _userRepository = userRepository;
    }

    public async Task<QuoteResponseDto> CreateQuoteAsync(QuoteRequestDto request, Guid userId)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        // Convert currency to USD
        var usdAmount = await _currencyService.ConvertToUsdAsync(request.Currency, request.Amount);

        // Create quote request entity
        var quoteRequest = new QuoteRequest
        {
            UserId = userId,
            Mode = request.Mode,
            MovementType = request.MovementType,
            Incoterms = request.Incoterms,
            PackageType = request.PackageType,
            Country = request.Country,
            City = request.City,
            OriginalCurrency = request.Currency,
            OriginalAmount = request.Amount,
            UsdAmount = usdAmount
        };

        quoteRequest = await _quoteRepository.CreateAsync(quoteRequest);

        return new QuoteResponseDto
        {
            Id = quoteRequest.Id,
            UserEmail = user.Email,
            Country = quoteRequest.Country,
            City = quoteRequest.City,
            Mode = quoteRequest.Mode,
            MovementType = quoteRequest.MovementType,
            Incoterms = quoteRequest.Incoterms,
            PackageType = quoteRequest.PackageType,
            OriginalCurrency = quoteRequest.OriginalCurrency,
            OriginalAmount = quoteRequest.OriginalAmount,
            UsdAmount = quoteRequest.UsdAmount,
            CreatedDate = quoteRequest.CreatedAt
        };
    }

    public async Task<IEnumerable<QuoteResponseDto>> GetAllQuotesAsync()
    {
        var quotes = await _quoteRepository.GetAllAsync();

        return quotes.Select(q => new QuoteResponseDto
        {
            Id = q.Id,
            UserEmail = q.User.Email,
            Country = q.Country,
            City = q.City,
            Mode = q.Mode,
            MovementType = q.MovementType,
            Incoterms = q.Incoterms,
            PackageType = q.PackageType,
            OriginalCurrency = q.OriginalCurrency,
            OriginalAmount = q.OriginalAmount,
            UsdAmount = q.UsdAmount,
            CreatedDate = q.CreatedAt
        });
    }
}

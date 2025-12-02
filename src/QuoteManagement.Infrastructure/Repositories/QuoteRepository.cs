using Microsoft.EntityFrameworkCore;
using QuoteManagement.Domain.Entities;
using QuoteManagement.Domain.Interfaces;
using QuoteManagement.Infrastructure.Data;

namespace QuoteManagement.Infrastructure.Repositories;

public class QuoteRepository : IQuoteRepository
{
    private readonly ApplicationDbContext _context;

    public QuoteRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<QuoteRequest> CreateAsync(QuoteRequest quoteRequest)
    {
        quoteRequest.Id = Guid.NewGuid();
        quoteRequest.CreatedAt = DateTime.UtcNow;
        
        _context.QuoteRequests.Add(quoteRequest);
        await _context.SaveChangesAsync();
        
        return quoteRequest;
    }

    public async Task<IEnumerable<QuoteRequest>> GetAllAsync()
    {
        return await _context.QuoteRequests
            .Include(q => q.User)
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();
    }

    public async Task<QuoteRequest?> GetByIdAsync(Guid id)
    {
        return await _context.QuoteRequests
            .Include(q => q.User)
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}

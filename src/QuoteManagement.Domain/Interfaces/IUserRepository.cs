using QuoteManagement.Domain.Entities;

namespace QuoteManagement.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User> CreateAsync(User user);
    Task<bool> ExistsAsync(string email);
}

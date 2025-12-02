namespace QuoteManagement.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(Guid userId, string email);
}

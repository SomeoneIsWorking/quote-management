using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuoteManagement.Application.DTOs;
using QuoteManagement.Application.Interfaces;

namespace QuoteManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class QuotesController : ControllerBase
{
    private readonly IQuoteService _quoteService;

    public QuotesController(IQuoteService quoteService)
    {
        _quoteService = quoteService;
    }

    [HttpPost]
    public async Task<ActionResult<QuoteResponseDto>> CreateQuote([FromBody] QuoteRequestDto request)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new { message = "Invalid user token" });
            }

            var response = await _quoteService.CreateQuoteAsync(request, userId);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuoteResponseDto>>> GetAllQuotes()
    {
        var quotes = await _quoteService.GetAllQuotesAsync();
        return Ok(quotes);
    }
}

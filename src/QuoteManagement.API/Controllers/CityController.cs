using Microsoft.AspNetCore.Mvc;
using QuoteManagement.Application.Interfaces;

namespace QuoteManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;

    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetCities([FromQuery] string country)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            return BadRequest(new { message = "Country parameter is required" });
        }

        var cities = await _cityService.GetCitiesByCountryAsync(country);
        return Ok(cities);
    }
}

using CitiesManager.WebAPI.DataBaseContext;
using CitiesManager.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.Controllers.v2;

public class CitiesController : CustomControllerBase
{
    private readonly ApplicationDbContext _context;

    public CitiesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Cities
    /// <summary>
    ///     Get all cities including city name and ID
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    // [Produces("application/xml")]
    public async Task<ActionResult<IEnumerable<string?>>> GetCities()
    {
        return await _context.Cities
            .OrderBy(x => x.CityName)
            .Select(x => x.CityName)
            .ToListAsync();
    }
}
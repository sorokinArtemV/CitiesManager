using Asp.Versioning;
using CitiesManager.WebAPI.DataBaseContext;
using CitiesManager.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.Controllers.v1;

[ApiVersion("1.0")]
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
    public async Task<ActionResult<IEnumerable<City>>> GetCities()
    {
        return await _context.Cities.OrderBy(x => x.CityName).ToListAsync();
    }

    // GET: api/Cities/5
    [HttpGet("{cityId}")]
    public async Task<ActionResult<City>> GetCity(Guid cityId)
    {
        var city = await _context.Cities.FindAsync(cityId);

        if (city == null) return Problem("City not found", statusCode: 400, title: "City search");

        return city;
    }

    // PUT: api/Cities/5
    [HttpPut("{cityId}")]
    public async Task<IActionResult> PutCity(Guid cityId, [Bind(nameof(City.CityId), nameof(City.CityName))] City city)
    {
        if (cityId != city.CityId) return BadRequest();

        var neededCity = await _context.Cities.FindAsync(cityId);

        if (neededCity is null) return NotFound();

        neededCity.CityName = city.CityName;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CityExists(cityId)) return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Cities
    [HttpPost]
    public async Task<ActionResult<City>> PostCity([Bind(nameof(City.CityId), nameof(City.CityName))] City city)
    {
        _context.Cities.Add(city);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetCity", new { cityId = city.CityId }, city);
    }

    // DELETE: api/Cities/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(Guid id)
    {
        var city = await _context.Cities.FindAsync(id);
        if (city == null) return NotFound();

        _context.Cities.Remove(city);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CityExists(Guid id)
    {
        return _context.Cities.Any(e => e.CityId == id);
    }
}
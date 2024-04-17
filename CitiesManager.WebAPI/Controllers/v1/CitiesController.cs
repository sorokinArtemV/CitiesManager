using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.DTO;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.Infrastucture.DataBaseContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.Controllers.v1;

[ApiVersion("1.0")]
// [EnableCors("4100Client")]
public class CitiesController : CustomControllerBase
{
    private readonly ICitiesAdderService _citiesAdderService;
    private readonly ICitiesDeleterService _citiesDeleterService;
    private readonly ICitiesGetterService _citiesGetterService;
    private readonly ICitiesUpdaterService _citiesUpdaterService;


    private readonly ApplicationDbContext _context;


    public CitiesController(
        ApplicationDbContext context,
        ICitiesGetterService citiesGetterService,
        ICitiesAdderService citiesAdderService,
        ICitiesUpdaterService citiesUpdaterService,
        ICitiesDeleterService citiesDeleterService)
    {
        _context = context;
        _citiesGetterService = citiesGetterService;
        _citiesAdderService = citiesAdderService;
        _citiesUpdaterService = citiesUpdaterService;
        _citiesDeleterService = citiesDeleterService;
    }

    // GET: api/Cities
    /// <summary>
    ///     Get all cities including city name and ID
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    // [Produces("application/xml")]
    public async Task<ActionResult<IEnumerable<CityDto>>> GetCities()
    {
        return await _citiesGetterService.GetAllCitiesAsync();
    }

    // GET: api/Cities/5
    [HttpGet("{cityId}")]
    public async Task<ActionResult<CityDto>> GetCity(Guid cityId)
    {
        var city = await _citiesGetterService.GetCityAsync(cityId);

        if (city == null) return Problem("City not found", statusCode: 400, title: "City search");

        return city;
    }

    // PUT: api/Cities/5
    [HttpPut("{cityId}")]
    public async Task<IActionResult> PutCity(Guid cityId,
        [Bind(nameof(City.CityId), nameof(City.CityName))]
        CityDto cityDto)
    {
        if (cityId != cityDto.CityId) return BadRequest();

        var neededCity = _citiesGetterService.GetCityAsync(cityId);

        if (neededCity is null) return NotFound();

        try
        {
            await _citiesUpdaterService.UpdateCityAsync(cityDto);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await CityExists(cityId) == false) return NotFound();
            throw;
        }

        return NoContent();
    }

    // POST: api/Cities
    [HttpPost]
    public async Task<ActionResult<CityDto>> PostCity(
        [Bind(nameof(CityDto.CityId), nameof(CityDto.CityName))]
        CityDto cityDto)
    {
        var city = await _citiesAdderService.AddCityAsync(cityDto);

        return CreatedAtAction("GetCity", new { cityId = city.CityId }, city);
    }

    // DELETE: api/Cities/5
    [HttpDelete("{cityId}")]
    public async Task<IActionResult> DeleteCity(Guid cityId)
    {
        var isCityDeleted = await _citiesDeleterService.DeleteCityAsync(cityId);

        if (!isCityDeleted) return NotFound();

        return NoContent();
    }

    private async Task<bool> CityExists(Guid id)
    {
        var city = await _citiesGetterService.GetCityAsync(id);
        return city is not null;
    }
}
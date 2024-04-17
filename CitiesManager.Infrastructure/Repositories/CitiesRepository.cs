using CitiesManager.Core.Domain.RepositoryContracts;
using CitiesManager.Core.Entities;
using CitiesManager.Infrastucture.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.Infrastucture.Repositories;

public class CitiesRepository : ICitiesRepository
{
    private readonly ApplicationDbContext _db;

    public CitiesRepository(ApplicationDbContext context)
    {
        _db = context;
    }

    public async Task<List<City>> GetCitiesAsync()
    {
        return await _db.Cities.ToListAsync();
    }

    public async Task<City?> GetCityAsync(Guid cityId)
    {
        var city = await _db.Cities.FirstOrDefaultAsync(x => x.CityId == cityId);

        return city;
    }

    public async Task<City> AddCityAsync(City city)
    {
        _db.Cities.Add(city);
        await _db.SaveChangesAsync();

        return city;
    }

    public async Task<City> UpdateCityAsync(City city)
    {
        var matchingCity = await _db.Cities.FirstOrDefaultAsync(x => x.CityId == city.CityId);

        if (matchingCity is null) return city;

        matchingCity.CityName = city.CityName;
        matchingCity.CityId = city.CityId;

        var rowsUpdated = await _db.SaveChangesAsync();

        return matchingCity;
    }

    public async Task<bool> DeleteCityAsync(Guid cityId)
    {
        _db.Cities.RemoveRange(_db.Cities.Where(x => x.CityId == cityId));
        var rowsDeleted = await _db.SaveChangesAsync();

        return rowsDeleted > 0;
    }
}
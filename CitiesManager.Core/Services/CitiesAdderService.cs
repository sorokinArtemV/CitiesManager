using CitiesManager.Core.Domain.RepositoryContracts;
using CitiesManager.Core.DTO;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.Core.Services;

public class CitiesAdderService : ICitiesAdderService
{
    private readonly ICitiesRepository _citiesRepository;


    public async Task<CityDto> AddCityAsync(CityDto? cityDto)
    {
        ArgumentNullException.ThrowIfNull(cityDto);
    }
}
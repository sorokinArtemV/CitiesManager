using CitiesManager.Core.Domain.RepositoryContracts;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.Core.Services;

public class CitiesDeleterService : ICitiesDeleterService
{
    private readonly ICitiesRepository _citiesRepository;

    public CitiesDeleterService(ICitiesRepository citiesRepository)
    {
        _citiesRepository = citiesRepository;
    }


    public async Task<bool> DeleteCityAsync(Guid cityId)
    {
        if (await _citiesRepository.GetCityAsync(cityId) is null) return false;

        await _citiesRepository.DeleteCityAsync(cityId);

        return true;
    }
}
using CitiesManager.Core.DTO;

namespace CitiesManager.Core.ServiceContracts;

/// <summary>
///     Interface for updating cities
/// </summary>
public interface ICitiesUpdaterService
{
    /// <summary>
    ///     Update city
    /// </summary>
    /// <param name="cityDto"></param>
    /// <returns></returns>
    public Task<CityDto?> UpdateCityAsync(CityDto? cityDto);
}
using CitiesManager.Core.DTO;

namespace CitiesManager.Core.ServiceContracts;

/// <summary>
///     Interface for adding cities
/// </summary>
public interface ICitiesAdderService
{
    /// <summary>
    ///     Add city
    /// </summary>
    /// <param name="cityDto">City data transfer object</param>
    /// <returns>Added city</returns>
    public Task<CityDto> AddCityAsync(CityDto? cityDto);
}
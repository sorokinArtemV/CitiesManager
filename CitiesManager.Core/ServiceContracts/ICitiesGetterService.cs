using CitiesManager.Core.DTO;

namespace CitiesManager.Core.ServiceContracts;

/// <summary>
///     Interface for getting cities
/// </summary>
public interface ICitiesGetterService
{
    /// <summary>
    ///     Get all cities
    /// </summary>
    /// <returns> List of cities</returns>
    Task<List<CityDto>> GetAllCitiesAsync();

    /// <summary>
    ///     Get city by id
    /// </summary>
    /// <param name="cityId">City id</param>
    /// <returns>City</returns>
    Task<CityDto?> GetCityAsync(Guid? cityId);
}
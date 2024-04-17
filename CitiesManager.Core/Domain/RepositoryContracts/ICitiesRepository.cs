using CitiesManager.Core.Domain.Entities;

namespace CitiesManager.Core.Domain.RepositoryContracts;

/// <summary>
///     Interface for working with cities
/// </summary>
public interface ICitiesRepository
{
    /// <summary>
    ///     Get all cities
    /// </summary>
    /// <returns>Returns list of cities </returns>
    Task<List<City>> GetCitiesAsync();
    
    /// <summary>
    ///     Get city by id
    /// </summary>
    /// <param name="cityId">City id</param>
    /// <returns>Returns city</returns>
    Task<City?> GetCityAsync(Guid cityId);
    
    /// <summary>
    ///     Add new city
    /// </summary>
    /// <param name="city">City object</param>
    /// <returns>Returns added city</returns>
    Task<City> AddCityAsync(City city);
    
    /// <summary>
    ///    Update city
    /// </summary>
    /// <param name="city">City object</param>
    /// <returns>Returns updated city</returns>
    Task<City> UpdateCityAsync(City city);
    
    /// <summary>
    ///     Delete city
    /// </summary>
    /// <param name="cityId">City id</param>
    /// <returns>True if city was deleted, otherwise false</returns>
    Task<bool> DeleteCityAsync(Guid cityId);
}
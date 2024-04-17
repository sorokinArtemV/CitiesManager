namespace CitiesManager.Core.ServiceContracts;

/// <summary>
///     Interface for deleting cities
/// </summary>
public interface ICitiesDeleterService
{
    /// <summary>
    ///     Delete city
    /// </summary>
    /// <param name="cityId"></param>
    /// <returns></returns>
    public Task<bool> DeleteCityAsync(Guid cityId);
}
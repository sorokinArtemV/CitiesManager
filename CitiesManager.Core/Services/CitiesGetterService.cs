using AutoMapper;
using CitiesManager.Core.Domain.RepositoryContracts;
using CitiesManager.Core.DTO;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.Core.Services;

public class CitiesGetterService : ICitiesGetterService
{
    private readonly ICitiesRepository _citiesRepository;
    private readonly IMapper _mapper;


    public CitiesGetterService(ICitiesRepository citiesRepository, IMapper mapper)
    {
        _citiesRepository = citiesRepository;
        _mapper = mapper;
    }

    public async Task<List<CityDto>> GetAllCitiesAsync()
    {
        var cities = await _citiesRepository.GetCitiesAsync();
        
        return _mapper.Map<List<CityDto>>(cities);
    }

    public async Task<CityDto?> GetCityAsync(Guid? cityId)
    {
        ArgumentNullException.ThrowIfNull(cityId);
        
        var city = await _citiesRepository.GetCityAsync(cityId.Value);
        
        return _mapper.Map<CityDto>(city);
    }
}
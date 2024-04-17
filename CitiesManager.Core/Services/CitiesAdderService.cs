using AutoMapper;
using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.Domain.RepositoryContracts;
using CitiesManager.Core.DTO;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.Core.Services;

public class CitiesAdderService : ICitiesAdderService
{
    private readonly ICitiesRepository _citiesRepository;
    private readonly IMapper _mapper;

    public CitiesAdderService(ICitiesRepository citiesRepository, IMapper mapper)
    {
        _citiesRepository = citiesRepository;
        _mapper = mapper;
    }


    public async Task<CityDto> AddCityAsync(CityDto? cityDto)
    {
        ArgumentNullException.ThrowIfNull(cityDto);
        
        var city = _mapper.Map<City>(cityDto);
        await _citiesRepository.AddCityAsync(city);
        
        return _mapper.Map<CityDto>(city);
    }
}
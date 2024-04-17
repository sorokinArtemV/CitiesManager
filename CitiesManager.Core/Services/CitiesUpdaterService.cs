using AutoMapper;
using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.Domain.RepositoryContracts;
using CitiesManager.Core.DTO;
using CitiesManager.Core.Exceptions;
using CitiesManager.Core.ServiceContracts;

namespace CitiesManager.Core.Services;

public class CitiesUpdaterService : ICitiesUpdaterService
{
    private readonly ICitiesRepository _citiesRepository;
    private readonly IMapper _mapper;
  

    public CitiesUpdaterService(ICitiesRepository citiesRepository, IMapper mapper)
    {
        _citiesRepository = citiesRepository;
        _mapper = mapper;
    }


    public async Task<CityDto> UpdateCityAsync(CityDto? cityDto)
    {
        ArgumentNullException.ThrowIfNull(cityDto);

        var neededCity = await _citiesRepository.GetCityAsync(cityDto.CityId);

        if (neededCity is null) throw new InvalidCityIdException("Invalid city ID");
        
        await _citiesRepository.UpdateCityAsync(_mapper.Map<City>(cityDto));
        
        return cityDto;
    }
}
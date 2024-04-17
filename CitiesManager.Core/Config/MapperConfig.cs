using AutoMapper;
using CitiesManager.Core.Domain.Entities;
using CitiesManager.Core.DTO;

namespace CitiesManager.Core.Config;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<City, CityDto>().ReverseMap();
    }
}
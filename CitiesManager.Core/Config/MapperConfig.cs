using AutoMapper;
using CitiesManager.Core.DTO;
using CitiesManager.Core.Entities;

namespace CitiesManager.Core.Config;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<City, CityDto>().ReverseMap();
    }
}
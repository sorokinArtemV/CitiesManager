using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models;

public class City
{
    [Key]
    public Guid CityId { get; set; }

    public string? Name { get; set; }
}
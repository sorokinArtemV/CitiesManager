using System.ComponentModel.DataAnnotations;

namespace CitiesManager.WebAPI.Models;

public class City
{
    [Key]
    public Guid CityId { get; set; }

    [Required(ErrorMessage = "City name is required")]
    public string? CityName { get; set; }
}
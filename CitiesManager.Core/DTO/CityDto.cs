using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.DTO;

public class CityDto
{
    [Required(ErrorMessage = "City ID is required")]
    public Guid CityId { get; set; }

    [Required(ErrorMessage = "City name is required")]
    public string? CityName { get; set; }
}
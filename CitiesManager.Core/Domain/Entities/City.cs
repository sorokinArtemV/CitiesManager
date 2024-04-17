using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.Entities;

public class City
{
    [Key]
    public Guid CityId { get; set; }

    [Required(ErrorMessage = "City name is required")]
    public string? CityName { get; set; }
}
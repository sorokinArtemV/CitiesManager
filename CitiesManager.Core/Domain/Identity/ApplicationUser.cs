using Microsoft.AspNetCore.Identity;

namespace CitiesManager.Core.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? PersonName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryDateTime { get; set; }
}
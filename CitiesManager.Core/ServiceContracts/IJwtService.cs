using System.Security.Claims;
using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;

namespace CitiesManager.Core.ServiceContracts;

public interface IJwtService
{
    AuthenticationResponse CreateJwtToken(ApplicationUser user);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}
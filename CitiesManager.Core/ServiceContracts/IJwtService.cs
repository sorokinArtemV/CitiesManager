using System.Security.Claims;
using CitiesManager.Core.Domain.Identity;
using CitiesManager.Core.DTO;

namespace CitiesManager.Core.ServiceContracts;

public interface IJwtService
{
    AuthenticationResponse CreateJwtToken(ApplicationUser user);
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}
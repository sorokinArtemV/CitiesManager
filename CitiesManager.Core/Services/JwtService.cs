using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CitiesManager.Core.Services;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public AuthenticationResponse CreateJwtToken(ApplicationUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));

        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // jwt unique identifier
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),

            // optional claims
            new Claim(ClaimTypes.NameIdentifier, user.Email), // unique identifier for user (email)
            new Claim(ClaimTypes.Name, user.PersonName) // unique identifier for user (name)
        ];

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var tokenGenerator = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: signingCredentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(tokenGenerator);

        return new AuthenticationResponse()
        {
            Token = token,
            Email = user.Email,
            PersonName = user.PersonName,
            Expiration = expiration
        };
    }
}
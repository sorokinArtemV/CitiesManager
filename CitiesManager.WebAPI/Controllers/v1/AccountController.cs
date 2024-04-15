using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using CitiesManager.WebAPI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers.v1;

/// <summary>
///     Account controller
/// </summary>
[AllowAnonymous]
[ApiVersion("1.0")]
public class AccountController : CustomControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;


    /// <summary>
    ///     Account controller
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="signInManager"></param>
    /// <param name="jwtService"></param>
    public AccountController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
    }

    /// <summary>
    ///     Register new user
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost]
    [TypeFilter(typeof(ModelValidationFilter))]
    public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDto registerDto)
    {
        var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            PersonName = registerDto.PersonName
        };

        // Create User
        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);

            var authResponse = _jwtService.CreateJwtToken(user);

            return Ok(authResponse);
        }

        {
            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return Problem(errorMessage);
        }
    }

    /// <summary>
    ///     Login
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [TypeFilter(typeof(ModelValidationFilter))]
    public async Task<IActionResult> PostLogin(LoginDto loginDto)
    {
        var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null) return NoContent();

            await _signInManager.SignInAsync(user, false);

            var authResponse = _jwtService.CreateJwtToken(user);


            return Ok(authResponse);
        }

        return Problem("Invalid email or password");
    }


    [HttpGet("logout")]
    public async Task<ActionResult<ApplicationUser>> GetLogout()
    {
        await _signInManager.SignOutAsync();
        return NoContent();
    }


    [HttpGet]
    public async Task<IActionResult> IsEmailAlreadyInUse(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);

        return user is null ? Ok(true) : Ok(false);
    }
}
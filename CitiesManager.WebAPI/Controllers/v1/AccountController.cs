using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
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
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    ///     Account controller
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="signInManager"></param>
    public AccountController(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    /// <summary>
    ///     Register new user
    /// </summary>
    /// <param name="registerDto"></param>
    /// <returns></returns>
    [HttpPost]
    // [TypeFilter(typeof(ModelValidationFilter))]
    public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = string.Join(", ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            return Problem(errorMessage);
        }

        var user = new ApplicationUser
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            PersonName = registerDto.PersonName
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, false);
            return Ok(user);
        }

        {
            var errorMessage = string.Join(", ", result.Errors);
            return Problem(errorMessage);
        }
    }

    /// <summary>
    /// Login
    /// </summary>
    /// <param name="loginDto"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<IActionResult> PostLogin(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessage = string.Join(", ", ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

            return Problem(errorMessage);
        }

        var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null) return NoContent();

            return Ok(new { user.PersonName, user.Email });
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
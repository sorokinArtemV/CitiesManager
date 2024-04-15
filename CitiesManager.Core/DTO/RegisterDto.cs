using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.Core.DTO;

public class RegisterDto
{
    [Required(ErrorMessage = "Person name is required")]
    public string PersonName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Remote("IsEmailInUse", "Account", ErrorMessage = "Email already in use")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression("^[0-9]{10}$", ErrorMessage = "Invalid phone number")]
    [Remote("IsEmailAlreadyInUse", "Account", ErrorMessage = "Email already in use")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone password is required")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}
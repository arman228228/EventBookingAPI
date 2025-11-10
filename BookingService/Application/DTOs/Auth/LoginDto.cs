using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth;

public class LoginDto
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public string Email { get; set; } = null!;
    
    [Required]
    [MinLength(6)]
    [MaxLength(32)]
    public string Password { get; set; } = null!;
}
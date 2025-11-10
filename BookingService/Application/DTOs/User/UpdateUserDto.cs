using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class UpdateUserDto
{
    [Required]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long")]
    [MaxLength(30, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; }
}
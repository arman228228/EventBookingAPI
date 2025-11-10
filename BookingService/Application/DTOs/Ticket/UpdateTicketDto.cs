using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class UpdateTicketDto
{
    public List<int> UserIds { get; set; }
    public int EventId { get; set; }
    
    [Range(500, 1_000_000, ErrorMessage = "Price must be between 500 and 1.000.000")]
    public int Price { get; set; }
}
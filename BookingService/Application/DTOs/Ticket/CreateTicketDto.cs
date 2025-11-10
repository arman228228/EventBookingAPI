using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class CreateTicketDto
{
    public List<int> UserIds { get; set; }
    public int EventId { get; set; }
    
    [Range(typeof(decimal), "500", "1000000", ErrorMessage = "Price must be between 500 and 1.000.000")]
    public decimal Price { get; set; }
}
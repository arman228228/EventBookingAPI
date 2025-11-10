namespace Application.DTOs;

public class EventDto
{
    public int Id { get; set; }
    
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int VenueId { get; set; }
}
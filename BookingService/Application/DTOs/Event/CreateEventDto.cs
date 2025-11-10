namespace Application.DTOs;

public class CreateEventDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int VenueId { get; set; }
}
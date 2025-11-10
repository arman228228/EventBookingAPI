namespace Domain.Entities;

public class Event
{
    public int Id { get; set; }
    
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int VenueId { get; set; }
    public Venue? Venue { get; set; }
    
    public EventDetails? Details { get; private set; }
}
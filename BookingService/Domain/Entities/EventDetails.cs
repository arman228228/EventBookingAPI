namespace Domain.Entities;

public class EventDetails
{
    public int EventId { get; set; }
    public Event Event { get; set; }

    public string? Description { get; set; }
    public string? OrganizerName { get; set; }
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
}
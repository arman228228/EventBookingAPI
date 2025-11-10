namespace Domain.Entities;

public class Venue
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public int SeatCapacity { get; set; }
}
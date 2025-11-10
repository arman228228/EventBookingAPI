namespace Domain.Entities;

public class Ticket
{
    public int Id { get; set; }

    public List<UserTicket> UserTickets { get; set; }
    public int EventId { get; set; }
    public Event Event { get; set; }
    public decimal Price { get; set; }
    
    public DateTime PurchasedAt { get; set; }
}
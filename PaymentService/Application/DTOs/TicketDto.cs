namespace Application.DTOs;

public class TicketDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchasedAt { get; set; }
}
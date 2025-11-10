namespace Application.DTOs;

public class CreatePaymentDto
{
    public int TicketId { get; set; }
    public decimal Amount { get; set; }
}
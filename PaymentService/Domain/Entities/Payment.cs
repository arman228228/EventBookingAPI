namespace Domain.Entities;

public enum PaymentStatus
{
    Pending,
    Approved,
    Declined
}

public class Payment
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public double Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
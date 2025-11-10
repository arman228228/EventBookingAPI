using Application.DTOs;

namespace Application.ResultPatterns;

public class TicketCreationResult
{
    public TicketDto? Ticket { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}
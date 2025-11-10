using Application.DTOs;

namespace Application.ResultPatterns;

public class PaymentCreationResult
{
    public bool Success { get; set; }
    public PaymentDto? PaymentDto { get; set; }
    public string? ErrorMessage { get; set; }
}
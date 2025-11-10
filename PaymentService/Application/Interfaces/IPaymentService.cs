using Application.DTOs;
using Application.ResultPatterns;

namespace Application.Interfaces;

public interface IPaymentService
{
    Task<PaymentCreationResult> CreateAsync(CreatePaymentDto dto);
    Task<PaymentDto?> GetByIdAsync(int id);
    Task<List<PaymentDto>> GetAllAsync();
}
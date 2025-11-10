using Domain.Entities;

namespace Application.Interfaces;

public interface IPaymentRepository
{
    Task<Payment> CreateAsync(Payment payment);
    Task<Payment?> GetByIdAsync(int id);
    Task<List<Payment>> GetAllAsync();
}
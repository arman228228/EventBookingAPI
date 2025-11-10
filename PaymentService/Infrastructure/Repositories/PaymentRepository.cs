using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Payment> CreateAsync(Payment payment)
    {
        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
        return payment;
    }
    
    public async Task<Payment> GetByIdAsync(int id)
    {
        return await _context.Payments.FindAsync(id);
    }
    
    public async Task<List<Payment>> GetAllAsync()
    {
        return await _context.Payments.ToListAsync();
    }
    
    
}
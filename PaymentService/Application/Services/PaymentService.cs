using Application.DTOs;
using Application.Interfaces;
using Application.ResultPatterns;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMapper _mapper;
    private readonly IBookingApiClient _bookingApiClient;
    
    public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, IBookingApiClient bookingApiClient)
    {
        _paymentRepository = paymentRepository;
        _mapper = mapper;
        _bookingApiClient = bookingApiClient;
    }
    
    public async Task<PaymentCreationResult> CreateAsync(CreatePaymentDto dto)
    {
        var ticket = await _bookingApiClient.GetTicketByIdAsync(dto.TicketId);
        if(ticket == null)
        {
            return new PaymentCreationResult
            {
                Success = false,
                ErrorMessage = "Ticket not found"
            };
        }
        
        if(dto.Amount < ticket.Price)
        {
            return new PaymentCreationResult
            {
                Success = false,
                ErrorMessage = "Insufficient funds"
            };
        }
        
        var payment = _mapper.Map<Payment>(dto);
        
        payment.Status = PaymentStatus.Pending;
        payment.CreatedAt = DateTime.UtcNow;
        
        var created = await _paymentRepository.CreateAsync(payment);

        return new PaymentCreationResult
        {
            Success = true,
            PaymentDto = _mapper.Map<PaymentDto>(created)
        };
    }
    
    public async Task<PaymentDto?> GetByIdAsync(int id)
    {
        var payment = await _paymentRepository.GetByIdAsync(id);
        return _mapper.Map<PaymentDto>(payment);
    }
    
    public async Task<List<PaymentDto>> GetAllAsync()
    {
        var payments = await _paymentRepository.GetAllAsync();
        return _mapper.Map<List<PaymentDto>>(payments);
    }
}
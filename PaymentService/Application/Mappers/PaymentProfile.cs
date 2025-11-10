using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

public class PaymentProfile : Profile
{
    public PaymentProfile()
    {
        CreateMap<Payment, PaymentDto>();
        CreateMap<CreatePaymentDto, Payment>();
    }
}
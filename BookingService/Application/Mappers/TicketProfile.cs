using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

public class TicketProfile : Profile
{
    public TicketProfile()
    {
        CreateMap<TicketDto, Ticket>();
        CreateMap<Ticket, TicketDto>()
            .ForMember(
                dest => dest.UserIds,
                opt => opt.MapFrom(src => src.UserTickets.Select(ut => ut.UserId))
            );
        
        CreateMap<CreateTicketDto, Ticket>();
        CreateMap<Ticket, CreateTicketDto>();
        
        CreateMap<UpdateTicketDto, Ticket>();
        CreateMap<Ticket, UpdateTicketDto>();

        CreateMap<PagedResult<Ticket>, PagedResult<TicketDto>>()
            .ForMember(
                dest => dest.Items, 
                opt => opt.MapFrom(src => src.Items)
            );
    }
}
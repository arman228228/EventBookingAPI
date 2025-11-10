using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<EventDto, Event>();
        CreateMap<Event, EventDto>();
        
        CreateMap<CreateEventDto, Event>();
        CreateMap<Event, CreateEventDto>();
        
        CreateMap<UpdateEventDto, Event>();
        CreateMap<Event, UpdateEventDto>();
    }
}
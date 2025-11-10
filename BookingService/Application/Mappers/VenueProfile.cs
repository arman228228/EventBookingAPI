using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

public class VenueProfile : Profile
{
    public VenueProfile()
    {
        CreateMap<VenueDto, Venue>();
        CreateMap<Venue, VenueDto>();
        
        CreateMap<CreateVenueDto, Venue>();
        CreateMap<Venue, CreateVenueDto>();
        
        CreateMap<UpdateVenueDto, Venue>();
        CreateMap<Venue, UpdateVenueDto>();
    }
}
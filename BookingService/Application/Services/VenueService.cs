using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class VenueService : IVenueService
{
    private readonly IVenueRepository _venueRepository;
    private readonly IMapper _mapper;
    
    public VenueService(IVenueRepository venueRepository, IMapper mapper)
    {
        _venueRepository = venueRepository;
        _mapper = mapper;
    }

    public async Task<VenueDto?> CreateAsync(CreateVenueDto request)
    {
        var venueEntity = _mapper.Map<Venue>(request);
        
        await _venueRepository.CreateAsync(venueEntity);
        
        return _mapper.Map<VenueDto>(venueEntity);
    }

    public async Task<List<VenueDto>> GetAllAsync() 
    {
        var venueEntities = await _venueRepository.GetAllAsync();
        return _mapper.Map<List<VenueDto>>(venueEntities);
    }
    
    public async Task<VenueDto?> GetByIdAsync(int id) 
    {
        var venueEntity = await _venueRepository.GetByIdAsync(id);
        return _mapper.Map<VenueDto>(venueEntity);
    }

    public async Task<UpdateVenueDto> UpdateAsync(int id, UpdateVenueDto request)
    {
        var entity = await _venueRepository.GetByIdAsync(id);
        if (entity == null)
        {
            return null;
        }

        _mapper.Map(request, entity);
        await _venueRepository.UpdateAsync(entity);
        return _mapper.Map<UpdateVenueDto>(entity);;
    }

    public async Task<bool> DeleteAsync(int id) => await _venueRepository.DeleteAsync(id);
}
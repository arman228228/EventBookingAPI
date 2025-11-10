using Application.DTOs;
using Application.ResultPatterns;
using Application.Interfaces;
using Application.Interfaces.Cache;
using Application.Interfaces.UnitOfWork;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class TicketService : ITicketService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    
    public TicketService(IUnitOfWork unitOfWork, ICacheService cacheService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<TicketCreationResult?> CreateAsync(CreateTicketDto request)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var eventDto = await _unitOfWork._events.GetByIdAsync(request.EventId);
            if (eventDto == null)
            {
                await _unitOfWork.RollbackAsync();
                
                return new TicketCreationResult
                {
                    Success = false,
                    ErrorMessage = $"Event {request.EventId} not found"
                };
            }

            var venueDto = await _unitOfWork._venues.GetByIdAsync(eventDto.VenueId);
            if (venueDto == null)
            {
                await _unitOfWork.RollbackAsync();
                
                return new TicketCreationResult
                {
                    Success = false,
                    ErrorMessage = $"Venue {eventDto.VenueId} not found"
                };
            }

            var ticketsCount = await _unitOfWork._tickets.CountByEventIdWithLockAsync(eventDto.Id);

            if (ticketsCount >= venueDto.SeatCapacity)
            {
                await _unitOfWork.RollbackAsync();
                
                return new TicketCreationResult
                {
                    Success = false,
                    ErrorMessage = $"No available seats"
                };
            }

            var users = await _unitOfWork._users.GetByIdsAsync(request.UserIds);

            if (users.Count != request.UserIds.Count)
            {
                await _unitOfWork.RollbackAsync();
                
                return new TicketCreationResult
                {
                    Success = false,
                    ErrorMessage = $"Not all users found"
                };
            }

            var ticketEntity = _mapper.Map<Ticket>(request);
            ticketEntity.PurchasedAt = DateTime.UtcNow;

            ticketEntity.UserTickets = request.UserIds.Select(uId => new UserTicket
            {
                UserId = uId
            }).ToList();

            await _unitOfWork._tickets.CreateAsync(ticketEntity);
            await _unitOfWork.SaveChangesAsync();
            
            await _unitOfWork.CommitAsync();

            var cacheKey = $"tickets:event:{request.EventId}";
            
            var newCount = ticketsCount + 1;
            await _cacheService.SetAsync(cacheKey, newCount.ToString(), TimeSpan.FromMinutes(5));
            
            return new TicketCreationResult
            {
                Success = true,
                Ticket = _mapper.Map<TicketDto>(ticketEntity)
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new TicketCreationResult
            {
                Success = false,
                ErrorMessage = $"Transaction exception"
            };
        }
    }

    public async Task<List<TicketDto>> GetAllAsync()
    {
        var ticketEntities = await _unitOfWork._tickets.GetAllAsync();
        return _mapper.Map<List<TicketDto>>(ticketEntities);
    }

    public async Task<TicketDto?> GetByIdAsync(int id) 
    {
        var ticketEntity = await _unitOfWork._tickets.GetByIdAsync(id);
        return _mapper.Map<TicketDto>(ticketEntity);
    }

    public async Task<List<TicketDto>> GetByIdsAsync(List<int> ticketIds)
    {
        var ticketEntities = await _unitOfWork._tickets.GetByIdsAsync(ticketIds);
        return _mapper.Map<List<TicketDto>>(ticketEntities);
    }
    
    public async Task<PagedResult<TicketDto>> GetTickets(int page = 1, int pageSize = 10)
    {
        if (page <= 0)
        {
            page = 1;
        }
        
        if (pageSize <= 0)
        {
            pageSize = 10;
        }
        
        var tickets = await _unitOfWork._tickets.GetTickets(page, pageSize);
        
        return _mapper.Map<PagedResult<TicketDto>>(tickets);
    }
    
    public async Task<UpdateTicketDto> UpdateAsync(int id, UpdateTicketDto request)
    {
        var entity = await _unitOfWork._tickets.GetByIdAsync(id);
        if (entity == null)
        {
            return null;
        }

        _mapper.Map(request, entity);
        await _unitOfWork._tickets.UpdateAsync(entity);
        return _mapper.Map<UpdateTicketDto>(entity);;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var ticket = await _unitOfWork._tickets.GetByIdAsync(id);
            if (ticket == null)
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }

            var eventId = ticket.EventId;
            var result = await _unitOfWork._tickets.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();

            if (!result)
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }

            await _unitOfWork.CommitAsync();

            var cacheKey = $"tickets:event:{eventId}";
            await _cacheService.DeleteAsync(cacheKey);

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }
    }
}
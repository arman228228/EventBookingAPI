using Application.DTOs;
using Application.ResultPatterns;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITicketService
{
    Task<TicketCreationResult?> CreateAsync(CreateTicketDto request);
    Task<List<TicketDto>> GetAllAsync();
    Task<TicketDto?> GetByIdAsync(int id);
    Task<List<TicketDto>> GetByIdsAsync(List<int> ticketIds);
    Task<PagedResult<TicketDto>> GetTickets(int page = 1, int pageSize = 10);
    Task<UpdateTicketDto> UpdateAsync(int id, UpdateTicketDto request);
    Task<bool> DeleteAsync(int id);
}
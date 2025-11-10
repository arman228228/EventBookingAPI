using Application.DTOs;

namespace Application.Interfaces;

public interface IBookingApiClient
{
    Task<TicketDto?> GetTicketByIdAsync(int ticketId);
}
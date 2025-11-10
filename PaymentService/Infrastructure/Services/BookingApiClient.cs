using System.Net.Http.Json;
using Application.DTOs;
using Application.Interfaces;

namespace Infrastructure.Services;

public class BookingApiClient : IBookingApiClient
{
    private readonly HttpClient _httpClient;
    
    public BookingApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TicketDto?> GetTicketByIdAsync(int ticketId)
    {
        var response = await _httpClient.GetAsync($"/api/ticket/{ticketId}");
        
        if(!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        return await response.Content.ReadFromJsonAsync<TicketDto>();
    }
}
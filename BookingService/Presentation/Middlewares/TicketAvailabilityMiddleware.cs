using System.Text.Json;
using Application.Interfaces;
using Application.Interfaces.Cache;
using Application.Services;

namespace Presentation.Middlewares;

public class TicketAvailabilityMiddleware
{
    private readonly RequestDelegate _next;
    
    public TicketAvailabilityMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == HttpMethods.Post
            && context.Request.Path.StartsWithSegments("/api/Ticket"))
        {
            context.Request.EnableBuffering();

            var json = await JsonDocument.ParseAsync(context.Request.Body);
            context.Request.Body.Position = 0;

            if (json.RootElement.TryGetProperty("eventId", out var EventId))
            {
                using var scope = context.RequestServices.CreateScope();
                var services = scope.ServiceProvider;

                var eventService = services.GetRequiredService<IEventService>();
                var venueService = services.GetRequiredService<IVenueService>();
                var ticketRepository = services.GetRequiredService<ITicketRepository>();
                
                var cacheService = services.GetRequiredService<ICacheService>();

                var eventId = EventId.GetInt32();

                var cacheKey = $"tickets:event:{eventId}";
                var cachedCount = await cacheService.GetAsync(cacheKey);
                
                int ticketsCount;

                if (cachedCount != null)
                {
                    ticketsCount = int.Parse(cachedCount);
                }
                else
                {
                    ticketsCount = await ticketRepository.CountByEventIdAsync(eventId);
                    await cacheService.SetAsync(cacheKey, ticketsCount.ToString(), TimeSpan.FromMinutes(5));
                }

                var eventDto = await eventService.GetByIdAsync(eventId);

                if (eventDto == null)
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("Event not found");
                    return;
                }
                
                var venueDto = await venueService.GetByIdAsync(eventDto.VenueId);
                
                if (venueDto == null)
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync("Venue not found");
                    return;
                }

                if (ticketsCount >= venueDto.SeatCapacity)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("No available seats");
                    return;
                }
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("EventId not found");
                return;
            }
        }
        
        await _next(context);
    }
}
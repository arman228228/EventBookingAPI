using System.Security.Claims;
using Application.Options;
using Microsoft.Extensions.Options;

namespace Presentation.Middlewares;

public class InternalServiceMiddleware
{
    private readonly RequestDelegate _next;

    public InternalServiceMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IOptions<InternalAuthOptions> options)
    {
        if (!context.User.Identity?.IsAuthenticated ?? true)
        {
            if (context.Request.Headers.TryGetValue("X-Internal-Secret", out var header) &&
                header == options.Value.Secret)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "service_payment"),
                    new Claim(ClaimTypes.Role, "InternalService")
                };
                context.User = new ClaimsPrincipal(
                    new ClaimsIdentity(claims, authenticationType: "InternalSecret"));
            }
        }

        await _next(context);
    }
}
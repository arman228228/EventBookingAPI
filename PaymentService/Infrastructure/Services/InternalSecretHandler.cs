using Application.Options;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class InternalSecretHandler : DelegatingHandler
{
    private readonly IOptions<BookingServiceOptions> _options;

    public InternalSecretHandler(IOptions<BookingServiceOptions> options)
    {
        _options = options;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Remove("X-Internal-Secret");
        request.Headers.Add("X-Internal-Secret", _options.Value.InternalSecret);
        return await base.SendAsync(request, cancellationToken);
    }
}
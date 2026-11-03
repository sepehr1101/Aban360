using Aban360.UserPool.Application.Features.SubscriberRequests;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Aban360.Api.Extensions;

public static class ConfigureSubscriberRequests
{
    public static void AddSubscriberRequests(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new SubscriberRequestOptions();
        configuration.GetSection("SubscriberRequests").Bind(options);
        services.AddSingleton(options);
        services.TryAddSingleton<TimeProvider>(TimeProvider.System);
        services.AddScoped<SubscriberRequestPayloadValidator>();
        services.AddScoped<ISubscriberRequestHandler, RegisterSubscriberRequestHandler>();
    }
}

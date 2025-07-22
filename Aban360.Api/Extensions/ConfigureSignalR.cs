using Aban360.Api.Hubs.Implementations;

namespace Aban360.Api.Extensions
{
    internal static class ConfigureSignalR
    {
        internal static void AddSignalrSupport(this IServiceCollection services)
        {
            services.AddSignalRCore();
        }
        internal static void UseSignalR(this IApplicationBuilder app)
        {
            //app.MapHub<NotifyHub>("notify-hub");
        }
    }
}

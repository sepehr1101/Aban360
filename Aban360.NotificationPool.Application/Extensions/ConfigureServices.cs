using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.NotificationPool.Application.Extensions
{
    public static class ConfigureServices
    {
        public static void AddNotificationPoolApplicationInjections(this IServiceCollection services)
        {           
            services.Scan(scan =>
                          scan
                            .FromAssemblies(Assembly.GetExecutingAssembly())
                            .AddClasses(publicOnly: false)
                            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                            .AsMatchingInterface()
                            .WithScopedLifetime());
        }
    }
}

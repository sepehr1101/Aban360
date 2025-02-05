using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.LocationPool.Application.Extensions
{
    public static class ConfigureServices
    {
        public static void AddLocationPoolApplicationInjections(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            // services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());

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

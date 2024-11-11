using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Aban360.UserPool.Persistence.Extensions
{
    public static class ConfigureServices
    {
        public static void AddPersistenceInjections(this IServiceCollection services)
        {            
            services.Scan(scan =>
                scan
                    .FromCallingAssembly()
                    .AddClasses(publicOnly: false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
        }
    }
}

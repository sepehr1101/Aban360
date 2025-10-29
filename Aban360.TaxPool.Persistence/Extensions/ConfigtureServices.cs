using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.TaxPool.Persistence.Extensions
{
    public static class ConfigtureServices
    {

        public static void AddTaxPoolPersistenceInjections(this IServiceCollection services)
        {
            services.Scan(scan =>
                        scan.
                            FromAssemblies(Assembly.GetExecutingAssembly())
                            .AddClasses(publicOnly: false)
                            .UsingRegistrationStrategy(RegistrationStrategy.Append)
                            .AsImplementedInterfaces()
                            .WithScopedLifetime());
        }
    }
}

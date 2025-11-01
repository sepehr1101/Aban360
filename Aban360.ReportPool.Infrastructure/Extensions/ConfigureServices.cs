using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.ReportPool.Infrastructure.Extensions
{
    public static class ConfigureServices
    {
        public static void AddReportPoolInfrastructureInjections(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
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

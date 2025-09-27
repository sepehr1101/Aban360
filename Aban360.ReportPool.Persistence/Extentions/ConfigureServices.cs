using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Reflection;

namespace Aban360.ReportPool.Persistence.Extentions
{
    public static class ConfigureServices
    {
        public static void AddReportPoolPersistenceInjections(this IServiceCollection services)
        {
            services.Scan(scan =>
                    scan.FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(publicOnly: false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
        }
    }
}

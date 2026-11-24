using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.LocationPool.Persistence.Extensions
{
    public static class ConfigureServices
    {
        public static void AddLocationPoolPersistenceInjections(this IServiceCollection services)
        {
            services.Scan(scan =>
                scan
                    .FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes => classes.Where(type => !ConfigureMergedRepositories.IsMergedRepository(type)), publicOnly: false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.AddMergedPersistenceRepositories();
        }
    }
}

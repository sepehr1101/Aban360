using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.InstallationPool.Persistence.Extensions
{
    public static class ConfigureServices
    {
        public static void AddInstallationPoolPersistenceInjections(this IServiceCollection services)
        {
            services.Scan(scan =>
                scan
                    .FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes => classes.InNamespaces("Aban360.InstallationPool.Persistence").Where(type => !ConfigureMergedRepositories.IsMergedRepository(type)), publicOnly: false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.AddMergedPersistenceRepositories();
        }
    }
}

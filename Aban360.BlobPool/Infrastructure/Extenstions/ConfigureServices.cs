using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban260.BlobPool.Infrastructure.Extenstions
{
    public static class ConfigureServices
    {
        public static void AddBlobPoolInfrastructureInjections(this IServiceCollection services)
        {
            services.AddAutoMapper(Array.Empty<Assembly>());
            services.Scan(scan =>
                          scan
                            .FromAssemblies(Assembly.GetExecutingAssembly())
                            .AddClasses(classes => classes.InNamespaces("Aban260.BlobPool.Infrastructure"), publicOnly: false)
                            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                            .AsMatchingInterface()
                            .WithScopedLifetime());

        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.BlobPool.Persistence.Extensions
{
    public static class ConfigureServices
    {
        public static void AddBlobPoolPersistenceInjections(this IServiceCollection services)
        {
            services.Scan(scan =>
                scan
                    .FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(publicOnly: false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
        }
    }
}

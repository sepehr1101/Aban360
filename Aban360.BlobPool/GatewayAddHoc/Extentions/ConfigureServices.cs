using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Scrutor;

namespace Aban360.BlobPool.GatewayAddHoc.Extentions
{
    public static class ConfigureServices
    {
        public static void AddBlobPoolGatewayInjections(this IServiceCollection services)
        {
            services.AddAutoMapper(Array.Empty<Assembly>());

            services.Scan(scan =>
                          scan
                            .FromAssemblies(Assembly.GetExecutingAssembly())
                            .AddClasses(classes => classes.InNamespaces("Aban360.BlobPool.GatewayAddHoc"), publicOnly: false)
                            .UsingRegistrationStrategy(RegistrationStrategy.Throw)
                            .AsMatchingInterface()
                            .WithScopedLifetime());
        }
    }
}

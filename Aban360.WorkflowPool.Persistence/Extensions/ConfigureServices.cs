using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.WorkflowPool.Persistence.Extensions
{
    public static class ConfigureServices
    {
<<<<<<< HEAD
        public static void AddWorkflowPoolPersistenceInjections(this IServiceCollection services)
=======
        public static void AddWorkflowPersistenceInjections(this IServiceCollection services)
>>>>>>> c37947236e070bdc467d5ea27c49cf971e406758
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

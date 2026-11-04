using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.UserPool.Persistence.Extensions
{
    public static class ConfigureServices
    {
        public static void AddUserPoolPersistenceInjections(this IServiceCollection services)
        {            
            services.Scan(scan =>
                scan
                    .FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes => classes.Where(type =>
                        !typeof(Aban360.UserPool.Domain.Features.SubscriberRequests.ISubscriberRequestTransaction).IsAssignableFrom(type)),
                        publicOnly: false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Append)
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
        }
    }
}

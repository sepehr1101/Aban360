using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.TaxPool.Application.Extensions
{
    public static class ConfigureServices
    {
        public static void AddTaxPoolApplicationInjections(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
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

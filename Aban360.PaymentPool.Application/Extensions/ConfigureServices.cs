using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Scrutor;

namespace Aban360.PaymentPool.Application.Extensions
{
    public static class ConfigureServices
    {
        public static void AddPaymentPoolApplicationInjections(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
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

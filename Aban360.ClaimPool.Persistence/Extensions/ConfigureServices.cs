using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;

namespace Aban360.ClaimPool.Persistence.Extensions
{
    public static class ConfigureServices
    {
        public static void AddClaimPoolPersistenceInjections(this IServiceCollection services)
        {
            // ثبت جداگانه تضمین می‌کند دو قرارداد هر Repository در یک Scope نمونه مشترک داشته باشند.
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
